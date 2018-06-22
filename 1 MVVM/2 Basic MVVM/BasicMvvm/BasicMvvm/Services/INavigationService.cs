using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMvvm.Services {
    /// <summary>
    /// 导航服务。
    /// </summary>
    public interface INavigationService {
        /// <summary>
        /// 返回。
        /// </summary>
        void GoBack();

        /// <summary>
        /// 导航。
        /// </summary>
        /// <param name="pageType">目标页面类型。</param>
        void NavigateTo(Type pageType);
    }
}