using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    public class VoteViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     投票服务。
        /// </summary>
        private readonly IVoteService _voteService;

        /// <summary>
        ///     载入命令。
        /// </summary>
        private RelayCommand _loadCommand;

        /// <summary>
        ///     正在载入。
        /// </summary>
        private bool _loading;

        /// <summary>
        ///     提交命令。
        /// </summary>
        private RelayCommand _submitCommand;

        /// <summary>
        ///     正在提交。
        /// </summary>
        private bool _submitting;

        /// <summary>
        ///     投票。
        /// </summary>
        private Vote _vote;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="voteService">投票服务。</param>
        public VoteViewModel(IDialogService dialogService,
            IVoteService voteService) {
            _dialogService = dialogService;
            _voteService = voteService;
        }

        /// <summary>
        ///     正在载入。
        /// </summary>
        public bool Loading {
            get => _loading;
            set => Set(nameof(Loading), ref _loading, value);
        }

        /// <summary>
        ///     投票id。
        /// </summary>
        public int VoteId { get; set; }

        /// <summary>
        ///     投票。
        /// </summary>
        public Vote Vote {
            get => _vote;
            set => Set(nameof(Vote), ref _vote, value);
        }

        /// <summary>
        ///     载入命令。
        /// </summary>
        public RelayCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new RelayCommand(async () => {
                SubmitCommand.RaiseCanExecuteChanged();

                Loading = true;
                _loadCommand.RaiseCanExecuteChanged();
                var serviceResult = await _voteService.GetAsync(VoteId);
                Loading = false;
                _loadCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        Vote = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }

                SubmitCommand.RaiseCanExecuteChanged();
            }));

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand =
                new RelayCommand(async () => { },
                    () => !Loading && !Submitting && Vote != null &&
                          Vote.Questionnaire.Deadline > DateTime.Now));

        /// <summary>
        ///     正在提交。
        /// </summary>
        public bool Submitting {
            get => _submitting;
            set => Set(nameof(Submitting), ref _submitting, value);
        }
    }
}