﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PresetPictureSettingsMode.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   Picture Settings Mode when adding presets
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrake.ApplicationServices.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Picture Settings Mode when adding presets
    /// </summary>
    public enum PresetPictureSettingsMode
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "Custom")]
        Custom,
        [Display(Name = "Source Maximum")]
        SourceMaximum,
    }
}