﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddPresetViewModel.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   The Add Preset View Model
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrakeWPF.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using HandBrake.ApplicationServices.Functions;
    using HandBrake.ApplicationServices.Model;
    using HandBrake.ApplicationServices.Services;
    using HandBrake.ApplicationServices.Services.Interfaces;
    using HandBrakeWPF.Services.Interfaces;
    using HandBrakeWPF.ViewModels.Interfaces;

    /// <summary>
    /// The Add Preset View Model
    /// </summary>
    [Export(typeof(IAddPresetViewModel))]
    public class AddPresetViewModel : ViewModelBase, IAddPresetViewModel
    {
        /// <summary>
        /// Backing field for the Preset Service
        /// </summary>
        private readonly IPresetService presetService;

        /// <summary>
        /// Backing field for the error service
        /// </summary>
        private readonly IErrorService errorService;

        /// <summary>
        /// Backing fields for Selected Picture settings mode.
        /// </summary>
        private PresetPictureSettingsMode selectedPictureSettingMode;

        /// <summary>
        /// Backging field for show custom inputs
        /// </summary>
        private bool showCustomInputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPresetViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">
        /// The window manager.
        /// </param>
        /// <param name="presetService">
        /// The Preset Service
        /// </param>
        /// <param name="errorService">
        /// The Error Service
        /// </param>
        public AddPresetViewModel(IWindowManager windowManager, IPresetService presetService, IErrorService errorService)
        {
            this.presetService = presetService;
            this.errorService = errorService;
            this.Title = "Add Preset";
            this.Preset = new Preset { IsBuildIn = false, IsDefault = false, Category = PresetService.UserPresetCatgoryName, UsePictureFilters = true};
            this.PictureSettingsModes = EnumHelper<PresetPictureSettingsMode>.GetEnumList();
        }

        /// <summary>
        /// Gets the Preset
        /// </summary>
        public Preset Preset { get; private set; }

        /// <summary>
        /// Gets or sets PictureSettingsModes.
        /// </summary>
        public IEnumerable<PresetPictureSettingsMode> PictureSettingsModes { get; set; }

        /// <summary>
        /// Gets or sets CustomWidth.
        /// </summary>
        public int CustomWidth { get; set; }

        /// <summary>
        /// Gets or sets CustomHeight.
        /// </summary>
        public int CustomHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowCustomInputs.
        /// </summary>
        public bool ShowCustomInputs
        {
            get
            {
                return this.showCustomInputs;
            }
            set
            {
                this.showCustomInputs = value;
                this.NotifyOfPropertyChange(() => this.ShowCustomInputs);
            }
        }

        /// <summary>
        /// Gets or sets SelectedPictureSettingMode.
        /// </summary>
        public PresetPictureSettingsMode SelectedPictureSettingMode
        {
            get
            {
                return this.selectedPictureSettingMode;
            }
            set
            {
                this.selectedPictureSettingMode = value;
                this.ShowCustomInputs = value == PresetPictureSettingsMode.Custom;

                if (SelectedPictureSettingMode == PresetPictureSettingsMode.Custom)
                {
                    this.Preset.Task.MaxHeight = null;
                    this.Preset.Task.MaxWidth = null;
                }

                if (SelectedPictureSettingMode == PresetPictureSettingsMode.Custom)
                {
                    this.Preset.Task.Width = this.CustomWidth;
                    this.Preset.Task.Height = this.CustomHeight;
                    this.Preset.Task.MaxHeight = null;
                    this.Preset.Task.MaxWidth = null;
                }

                if (SelectedPictureSettingMode == PresetPictureSettingsMode.SourceMaximum)
                {
                    this.Preset.Task.MaxWidth = this.Preset.Task.Width;
                    this.Preset.Task.MaxHeight = this.Preset.Task.Height;
                }
            }
        }

        /// <summary>
        /// Prepare the Preset window to create a Preset Object later.
        /// </summary>
        /// <param name="task">
        /// The Encode Task.
        /// </param>
        public void Setup(EncodeTask task)
        {
            this.Preset.Task = new EncodeTask(task);
        }

        /// <summary>
        /// Add a Preset
        /// </summary>
        public void Add()
        {
            if (string.IsNullOrEmpty(this.Preset.Name))
            {
                this.errorService.ShowMessageBox("A Preset must have a Name. Please fill out the Preset Name field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.presetService.CheckIfPresetExists(this.Preset.Name))
            {
               MessageBoxResult result = this.errorService.ShowMessageBox("A Preset with this name already exists. Would you like to overwrite it?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
               if (result == MessageBoxResult.No)
               {
                   return;
               }
            }

            this.Preset.UsePictureFilters = this.Preset.UsePictureFilters;
            this.Preset.PictureSettingsMode = this.SelectedPictureSettingMode;

            bool added = this.presetService.Add(this.Preset);
            if (!added)
            {
                this.errorService.ShowMessageBox("Unable to add preset", "Unknown Error", MessageBoxButton.OK,
                                                 MessageBoxImage.Error);
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Cancel adding a preset
        /// </summary>
        public void Cancel()
        {
            this.Close();
        }

        /// <summary>
        /// Close this window.
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }
    }
}
