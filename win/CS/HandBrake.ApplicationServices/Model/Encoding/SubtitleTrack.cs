﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubtitleTrack.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   Subtitle Information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrake.ApplicationServices.Model.Encoding
{
    using System;

    using Caliburn.Micro;

    using HandBrake.ApplicationServices.Parsing;

    /// <summary>
    /// Subtitle Information
    /// </summary>
    public class SubtitleTrack : PropertyChangedBase
    {
        #region Constants and Fields

        /// <summary>
        /// The burned in backing field.
        /// </summary>
        private bool burned;

        /// <summary>
        /// The is default backing field.
        /// </summary>
        private bool isDefault;

        /// <summary>
        /// The source track.
        /// </summary>
        private Subtitle sourceTrack;

        /// <summary>
        /// Backing field for the srt file name.
        /// </summary>
        private string srtFileName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleTrack"/> class.
        /// </summary>
        public SubtitleTrack()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleTrack"/> class.
        /// Copy Constructor
        /// </summary>
        /// <param name="subtitle">
        /// The subtitle.
        /// </param>
        public SubtitleTrack(SubtitleTrack subtitle)
        {
            this.Burned = subtitle.Burned;
            this.Default = subtitle.Default;
            this.Forced = subtitle.Forced;
            this.sourceTrack = subtitle.SourceTrack;
            this.SrtCharCode = subtitle.SrtCharCode;
            this.SrtFileName = subtitle.SrtFileName;
            this.SrtLang = subtitle.SrtLang;
            this.SrtOffset = subtitle.SrtOffset;
            this.SrtPath = subtitle.SrtPath;
            this.SubtitleType = subtitle.SubtitleType;
            this.SourceTrack = subtitle.SourceTrack;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether Burned.
        /// </summary>
        public bool Burned
        {
            get
            {
                return this.burned;
            }

            set
            {
                this.burned = value;
                this.NotifyOfPropertyChange(() => this.Burned);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether Default.
        /// </summary>
        public bool Default
        {
            get
            {
                return this.isDefault;
            }

            set
            {
                this.isDefault = value;
                this.NotifyOfPropertyChange(() => this.Default);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether Forced.
        /// </summary>
        public bool Forced { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this is an SRT subtitle.
        /// </summary>
        public bool IsSrtSubtitle
        {
            get
            {
                return this.SrtFileName != "-" && this.SrtFileName != null;
            }
        }

        /// <summary>
        ///   Gets or sets SourceTrack.
        /// </summary>
        public Subtitle SourceTrack
        {
            get
            {
                return this.sourceTrack;
            }

            set
            {
                this.sourceTrack = value;
                this.NotifyOfPropertyChange(() => this.SourceTrack);
                if (this.sourceTrack != null)
                {
                    this.Track = this.sourceTrack.ToString();
                }
            }
        }

        /// <summary>
        ///   Gets or sets the SRT Character Code
        /// </summary>
        public string SrtCharCode { get; set; }

        /// <summary>
        ///   Gets or sets the SRT Filename
        /// </summary>
        public string SrtFileName
        {
            get
            {
                return srtFileName;
            }

            set
            {
                srtFileName = value;
                this.NotifyOfPropertyChange(() => this.IsSrtSubtitle);
            }
        }

        /// <summary>
        ///   Gets or sets the SRT Language
        /// </summary>
        public string SrtLang { get; set; }

        /// <summary>
        ///   Gets or sets the SRT Offset
        /// </summary>
        public int SrtOffset { get; set; }

        /// <summary>
        ///   Gets or sets the Path to the SRT file
        /// </summary>
        public string SrtPath { get; set; }

        /// <summary>
        ///   Gets or sets the type of the subtitle
        /// </summary>
        public SubtitleType SubtitleType { get; set; }

        /// <summary>
        ///   Gets or sets Track.
        /// </summary>
        [Obsolete("Use SourceTrack Instead")]
        public string Track { get; set; }

        #endregion
    }
}