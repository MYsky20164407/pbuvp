using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WindowsCommunityToolkit.Design;
using WindowsCommunityToolkit.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace WindowsCommunityToolkit.ViewModels {
    /// <summary>
    /// ViewModel定位器。
    /// </summary>
    public class ViewModelLocator {
        /******** 私有变量 ********/

        /******** 公开属性 ********/

        /// <summary>
        /// 主页ViewModel。
        /// </summary>
        public MainPageViewModel MainPageViewModel =>
            // 更抽象，但不显示设计时数据。
            // ServiceLocator.Current.GetInstance<MainPageViewModel>();
            // 不抽象，但显示设计时数据。
            SimpleIoc.Default.GetInstance<MainPageViewModel>();

        /******** 继承方法 ********/

        /******** 公开方法 ********/

        /// <summary>
        /// 构造函数。
        /// </summary>
        public ViewModelLocator() {
            if (DesignMode.DesignModeEnabled) {
                SimpleIoc.Default
                    .Register<IContactService, DesignContactService>();
            } else {
                SimpleIoc.Default.Register<IContactService, ContactService>();
            }

            SimpleIoc.Default.Register<MainPageViewModel>();
        }

        /******** 私有方法 ********/
    }
}