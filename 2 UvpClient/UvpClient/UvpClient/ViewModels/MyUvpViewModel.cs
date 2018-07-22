using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;

namespace UvpClient.ViewModels {
    /// <summary>
    /// 我的uvp ViewModel。
    /// </summary>
    public class MyUvpViewModel : ViewModelBase {
        /// <summary>
        /// 我的uvp。
        /// </summary>
        private MyUvp _myUvp;

        /// <summary>
        /// 我的uvp。
        /// </summary>
        public MyUvp MyUvp {
            get => _myUvp;
            set => Set(nameof(MyUvp), ref _myUvp, value);
        }

        /// <summary>
        /// 刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                // TODO
            }));
    }
}