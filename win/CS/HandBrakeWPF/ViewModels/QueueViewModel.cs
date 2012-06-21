﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueViewModel.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   The Preview View Model
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrakeWPF.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using HandBrake.ApplicationServices;
    using HandBrake.ApplicationServices.EventArgs;
    using HandBrake.ApplicationServices.Model;
    using HandBrake.ApplicationServices.Services.Interfaces;

    using HandBrakeWPF.Services.Interfaces;
    using HandBrakeWPF.ViewModels.Interfaces;

    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// The Preview View Model
    /// </summary>
    [Export(typeof(IQueueViewModel))]
    public class QueueViewModel : ViewModelBase, IQueueViewModel
    {
        #region Constants and Fields

        /// <summary>
        /// The Error Service Backing field
        /// </summary>
        private readonly IErrorService errorService;

        /// <summary>
        /// The User Setting Service Backing Field.
        /// </summary>
        private readonly IUserSettingService userSettingService;

        /// <summary>
        /// Queue Processor Backing field
        /// </summary>
        private readonly IQueueProcessor queueProcessor;

        /// <summary>
        /// IsEncoding Backing field
        /// </summary>
        private bool isEncoding;

        /// <summary>
        /// Job Status Backing field.
        /// </summary>
        private string jobStatus;

        /// <summary>
        /// Jobs pending backing field
        /// </summary>
        private string jobsPending;

        /// <summary>
        /// Backing field for the when done action description
        /// </summary>
        private string whenDoneAction;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">
        /// The window manager.
        /// </param>
        /// <param name="userSettingService">
        /// The user Setting Service.
        /// </param>
        /// <param name="queueProcessor">
        /// The Queue Processor Service 
        /// </param>
        /// <param name="errorService">
        /// The Error Service 
        /// </param>
        public QueueViewModel(IWindowManager windowManager, IUserSettingService userSettingService, IQueueProcessor queueProcessor, IErrorService errorService)
        {
            this.userSettingService = userSettingService;
            this.queueProcessor = queueProcessor;
            this.errorService = errorService;
            this.Title = "Queue";
            this.JobsPending = "No encodes pending";
            this.JobStatus = "There are no jobs currently encoding";  
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsEncoding.
        /// </summary>
        public bool IsEncoding
        {
            get
            {
                return this.isEncoding;
            }

            set
            {
                this.isEncoding = value;
                this.NotifyOfPropertyChange(() => IsEncoding);
            }
        }

        /// <summary>
        /// Gets or sets JobStatus.
        /// </summary>
        public string JobStatus
        {
            get
            {
                return this.jobStatus;
            }

            set
            {
                this.jobStatus = value;
                this.NotifyOfPropertyChange(() => this.JobStatus);
            }
        }

        /// <summary>
        /// Gets or sets JobsPending.
        /// </summary>
        public string JobsPending
        {
            get
            {
                return this.jobsPending;
            }

            set
            {
                this.jobsPending = value;
                this.NotifyOfPropertyChange(() => this.JobsPending);
            }
        }

        /// <summary>
        /// Gets QueueJobs.
        /// </summary>
        public ObservableCollection<QueueTask> QueueJobs
        {
            get
            {
                return this.queueProcessor.QueueManager.Queue;
            }
        }

        /// <summary>
        /// Gets or sets WhenDoneAction.
        /// </summary>
        public string WhenDoneAction
        {
            get
            {
                return this.whenDoneAction;
            }
            set
            {
                this.whenDoneAction = value;
                this.NotifyOfPropertyChange(() => this.WhenDoneAction);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the When Done Setting
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void WhenDone(string action)
        {
            this.WhenDoneAction = action;
            this.userSettingService.SetUserSetting(ASUserSettingConstants.WhenCompleteAction, action);
        }

        /// <summary>
        /// Clear the Queue
        /// </summary>
        public void Clear()
        {
            this.queueProcessor.QueueManager.Clear();
        }

        /// <summary>
        /// Clear Completed Items
        /// </summary>
        public void ClearCompleted()
        {
            this.queueProcessor.QueueManager.ClearCompleted();
        }

        /// <summary>
        /// Close this window.
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        /// Handle the On Window Load
        /// </summary>
        public override void OnLoad()
        {
            // Setup the window to the correct state.
            this.IsEncoding = this.queueProcessor.EncodeService.IsEncoding;
            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);

            base.OnLoad();
        }

        /// <summary>
        /// Pause Encode
        /// </summary>
        public void PauseEncode()
        {
            this.queueProcessor.Pause();
        }

        /// <summary>
        /// Remove a Job from the queue
        /// </summary>
        /// <param name="task">
        /// The Job to remove from the queue
        /// </param>
        public void RemoveJob(QueueTask task)
        {
            if (task.Status == QueueItemStatus.InProgress)
            {
                MessageBoxResult result =
                    this.errorService.ShowMessageBox(
                        "This encode is currently in progress. If you delete it, the encode will be stoped. Are you sure you wish to proceed?", 
                        "Warning", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    this.queueProcessor.EncodeService.Stop();
                    this.queueProcessor.QueueManager.Remove(task);
                }
            }
            else
            {
                this.queueProcessor.QueueManager.Remove(task);
            }

            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);
        }

        /// <summary>
        /// Reset the job state to waiting.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        public void RetryJob(QueueTask task)
        {
            task.Status = QueueItemStatus.Waiting;
            this.queueProcessor.QueueManager.BackupQueue(null);
        }

        /// <summary>
        /// Start Encode
        /// </summary>
        public void StartEncode()
        {
            if (this.queueProcessor.QueueManager.Count == 0)
            {
                this.errorService.ShowMessageBox(
                    "There are no pending jobs.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.queueProcessor.Start();
        }

        /// <summary>
        /// Export the Queue to a file.
        /// </summary>
        public void Export()
        {
            VistaSaveFileDialog dialog = new VistaSaveFileDialog
                {
                    Filter = "HandBrake Queue Files (*.hbq)|*.hbq",
                    OverwritePrompt = true,
                    DefaultExt = ".hbq",
                    AddExtension = true
                };
            dialog.ShowDialog();

            this.queueProcessor.QueueManager.BackupQueue(dialog.FileName);
        }

        /// <summary>
        /// Import a saved queue
        /// </summary>
        public void Import()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog { Filter = "HandBrake Queue Files (*.hbq)|*.hbq", CheckFileExists = true  };
            dialog.ShowDialog();

            this.queueProcessor.QueueManager.RestoreQueue(dialog.FileName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override the OnActive to run the Screen Loading code in the view model base.
        /// </summary>
        protected override void OnActivate()
        {
            this.Load();

            this.WhenDoneAction = this.userSettingService.GetUserSetting<string>(ASUserSettingConstants.WhenCompleteAction);    

            this.queueProcessor.JobProcessingStarted += this.queueProcessor_JobProcessingStarted;
            this.queueProcessor.QueueCompleted += this.queueProcessor_QueueCompleted;
            this.queueProcessor.QueuePaused += this.queueProcessor_QueuePaused;
            this.queueProcessor.QueueManager.QueueChanged += this.QueueManager_QueueChanged;
            this.queueProcessor.EncodeService.EncodeStatusChanged += this.EncodeService_EncodeStatusChanged;

            base.OnActivate();
        }

        /// <summary>
        /// Override the Deactivate
        /// </summary>
        /// <param name="close">
        /// The close.
        /// </param>
        protected override void OnDeactivate(bool close)
        {
            this.queueProcessor.JobProcessingStarted -= this.queueProcessor_JobProcessingStarted;
            this.queueProcessor.QueueCompleted -= this.queueProcessor_QueueCompleted;
            this.queueProcessor.QueuePaused -= this.queueProcessor_QueuePaused;
            this.queueProcessor.QueueManager.QueueChanged -= this.QueueManager_QueueChanged;
            this.queueProcessor.EncodeService.EncodeStatusChanged -= this.EncodeService_EncodeStatusChanged;

            base.OnDeactivate(close);
        }

        /// <summary>
        /// Handle the Encode Status Changed Event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EncodeProgressEventArgs.
        /// </param>
        private void EncodeService_EncodeStatusChanged(
            object sender, EncodeProgressEventArgs e)
        {
            this.JobStatus =
                string.Format(
                    "Encoding: Pass {0} of {1},  {2:00.00}%, FPS: {3:000.0},  Avg FPS: {4:000.0},  Time Remaining: {5},  Elapsed: {6:hh\\:mm\\:ss}", 
                    e.Task, 
                    e.TaskCount, 
                    e.PercentComplete, 
                    e.CurrentFrameRate, 
                    e.AverageFrameRate, 
                    e.EstimatedTimeLeft, 
                    e.ElapsedTime);
        }

        /// <summary>
        /// Handle the Queue Changed Event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void QueueManager_QueueChanged(object sender, EventArgs e)
        {
            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);
        }

        /// <summary>
        /// Handle teh Job Processing Started Event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The QueueProgressEventArgs.
        /// </param>
        private void queueProcessor_JobProcessingStarted(
            object sender, QueueProgressEventArgs e)
        {
            this.JobStatus = "Queue Started";
            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);
            this.IsEncoding = true;
        }

        /// <summary>
        /// Handle the Queue Completed Event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void queueProcessor_QueueCompleted(object sender, EventArgs e)
        {
            this.JobStatus = "Queue Completed";
            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);
            this.IsEncoding = false;
        }

        /// <summary>
        /// Handle the Queue Paused Event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void queueProcessor_QueuePaused(object sender, EventArgs e)
        {
            this.JobStatus = "Queue Paused";
            this.JobsPending = string.Format("{0} jobs pending", this.queueProcessor.QueueManager.Count);
            this.IsEncoding = false;
        }

        #endregion
    }
}