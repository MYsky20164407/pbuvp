using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    public class VoteViewModel : ViewModelBase {
        /// <summary>
        ///     答案已提交信息。
        /// </summary>
        public const string AnswerSubmittedMessage =
            "Your answer has been submitted.";

        /// <summary>
        ///     答案为空错误信息。
        /// </summary>
        public const string AnswerCollectionEmptyErrorMessage =
            "Please choose at least on answer.";

        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _navigationService;

        /// <summary>
        ///     投票服务。
        /// </summary>
        private readonly IVoteService _voteService;

        /// <summary>
        ///     是否可以提交。
        /// </summary>
        private bool _canSubmit;

        /// <summary>
        ///     提交命令。
        /// </summary>
        private RelayCommand _submitCommand;

        /// <summary>
        ///     投票。
        /// </summary>
        private Vote _vote;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="voteService">投票服务。</param>
        /// <param name="navigationService">导航服务。</param>
        public VoteViewModel(IDialogService dialogService,
            IVoteService voteService,
            IRootNavigationService navigationService) {
            _dialogService = dialogService;
            _voteService = voteService;
            _navigationService = navigationService;
        }

        /// <summary>
        ///     投票。
        /// </summary>
        public Vote Vote {
            get => _vote;
            set {
                CanSubmit = value != null &&
                            value.Questionnaire.Deadline > DateTime.Now;
                Set(nameof(Vote), ref _vote, value);
            }
        }

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                if (Vote.AnswerCollection.Count == 0) {
                    await _dialogService.ShowAsync(
                        AnswerCollectionEmptyErrorMessage);
                    return;
                }

                CanSubmit = false;

                var serviceResult = await _voteService.SubmitAsync(Vote);

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.NoContent:
                        await _dialogService.ShowAsync(AnswerSubmittedMessage);
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

                CanSubmit = Vote != null &&
                            Vote.Questionnaire.Deadline > DateTime.Now;
            }));

        /// <summary>
        ///     是否可以提交。
        /// </summary>
        public bool CanSubmit {
            get => _canSubmit;
            set => Set(nameof(CanSubmit), ref _canSubmit, value);
        }
    }
}