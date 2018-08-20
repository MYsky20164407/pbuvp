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
        /// 组内自评/互评表已提交信息。
        /// </summary>
        public static string PeerWorkGroupEvaluationSubmittedMessage =
            "Your evaluation has been submitted.";

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
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                Submitting = true;
                _submitCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _peerWorkGroupEvaluationService.SubmitAsync(
                        PeerWorkGroupEvaluation);
                Submitting = false;
                _submitCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.NoContent:
                        await _dialogService.ShowAsync(
                            PeerWorkGroupEvaluationSubmittedMessage);
                        _tileService.ForceUpdate();
                        _navigationService.GoBack();
                        break;
                    case ServiceResultStatus.BadRequest:
                        await _dialogService.ShowAsync(serviceResult.Message);
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }, () => !Submitting));

        /// <summary>
        /// 正在提交。
        /// </summary>
        public bool Submitting {
            get => _submitting;
            set => Set(nameof(Submitting), ref _submitting, value);
        }
    }
}