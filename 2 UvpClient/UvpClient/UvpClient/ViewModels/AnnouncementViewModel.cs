using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using UvpClient.Models;

namespace UvpClient.ViewModels {
    /// <summary>
    /// 通知ViewModel。
    /// </summary>
    public class AnnouncementViewModel : ViewModelBase {
        /// <summary>
        /// 通知。
        /// </summary>
        private Announcement _announcement;

        /// <summary>
        /// 通知。
        /// </summary>
        public Announcement Announcement {
            get => _announcement;
            set => Set(nameof(Announcement), ref _announcement, value);
        }
    }
}