/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MCS"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AutoMapper;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MCS.Helpers;
using MCS.Repositories;
using MCS.Services;

namespace MCS.ViewModel
{
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			////if (ViewModelBase.IsInDesignModeStatic)
			////{
			////    // Create design time view services and models
			////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
			////}
			////else
			////{
			////    // Create run time view services and models
			////    SimpleIoc.Default.Register<IDataService, DataService>();
			////}

			var config = new AutoMapperConfiguration().Configure();
			var mapper = config.CreateMapper();

			SimpleIoc.Default.Register<IMapper>(() => mapper);
			
			SimpleIoc.Default.Register<IMessageStringManager, MessageStringManager>();
			SimpleIoc.Default.Register<IPathProvider, PathProvider>();
			SimpleIoc.Default.Register<IXmlService, XmlService>();
			SimpleIoc.Default.Register<IPersonRepository, PersonRepository>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}