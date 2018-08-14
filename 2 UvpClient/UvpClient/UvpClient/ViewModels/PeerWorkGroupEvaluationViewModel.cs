using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     组内自评/互评表ViewModel。
    /// </summary>
    public class PeerWorkGroupEvaluationViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _navigationService;

        /// <summary>
        ///     组内自评/互评表服务。
        /// </summary>
        private readonly IPeerWorkGroupEvaluationService
            _peerWorkGroupEvaluationService;

        /// <summary>
        ///     磁贴服务。
        /// </summary>
        private readonly ITileService _tileService;

        /// <summary>
        ///     组内自评/互评表。
        /// </summary>
        private PeerWorkGroupEvaluation _peerWorkGroupEvaluation;

        /// <summary>
        ///     提交命令。
        /// </summary>
        private RelayCommand _submitCommand;

        /// <summary>
        ///     是否可以提交。
        /// </summary>
        private bool _submitting;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="navigationService">导航服务。</param>
        /// <param name="peerWorkGroupEvaluationService">组内自评/互评表服务。</param>
        /// <param name="tileService">磁贴服务。</param>
        public PeerWorkGroupEvaluationViewModel(IDialogService dialogService,
            IRootNavigationService navigationService,
            IPeerWorkGroupEvaluationService peerWorkGroupEvaluationService,
            ITileService tileService) {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _peerWorkGroupEvaluationService = peerWorkGroupEvaluationService;
            _tileService = tileService;
        }

        /// <summary>
        ///     组内自评/互评表。
        /// </summary>
        public PeerWorkGroupEvaluation PeerWorkGroupEvaluation {
            get => _peerWorkGroupEvaluation;
            set => _peerWorkGroupEvaluation = value;
        }

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ??
            (_submitCommand = new RelayCommand(async () => {
                // TODO
            }));
    }
}