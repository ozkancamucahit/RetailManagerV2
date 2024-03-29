﻿using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;
using TRMDesktopUI.ViewModels;

namespace TRMDesktopUI
{
	public class Bootstrapper : BootstrapperBase
	{

		#region FIELDS
		private readonly SimpleContainer _container = new SimpleContainer();

		#endregion

		public Bootstrapper()
		{
			Initialize();

			ConventionManager.AddElementConvention<PasswordBox>(
			PasswordBoxHelper.BoundPasswordProperty,
			"Password",
			"PasswordChanged");
		}

		private IMapper ConfigureAutoMapper()
		{
			var config = new MapperConfiguration(cfg => {
				cfg.CreateMap<ProductModel, ProductDisplayModel>();
				cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
			});

			IMapper mapper = config.CreateMapper();
			return mapper;
		}


		protected override void Configure()
		{

			IMapper mapper = ConfigureAutoMapper();

			_container.Instance(mapper);

			_container
				.Instance(_container)
				.PerRequest<IProductEndPoint, ProductEndPoint>()
				.PerRequest<ISaleEndPoint, SaleEndPoint>()
				.PerRequest<IUserEndpoint, UserEndpoint>();

			_container
					.Singleton<IWindowManager, WindowManager>()
					.Singleton<IEventAggregator, EventAggregator>()
					.Singleton<ILoggedInUserModel, LoggedInUserModel>()
					.Singleton<IConfigHelper, ConfigHelper>()
					.Singleton<IAPIHelper, APIHelper>();

			GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => _container.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewForAsync<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
