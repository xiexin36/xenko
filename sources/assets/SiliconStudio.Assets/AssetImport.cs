// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.ComponentModel;
using SiliconStudio.Core;
using SiliconStudio.Core.IO;

namespace SiliconStudio.Assets
{
    /// <summary>
    /// An importable asset.
    /// </summary>
    [DataContract]
    public abstract class AssetImport : Asset
    {
        protected AssetImport()
        {
            Source = new UFile("");    
        }

        /// <summary>
        /// Gets or sets the source file of this asset.
        /// </summary>
        /// <value>The source.</value>
        /// <userdoc>
        /// The source file of this asset.
        /// </userdoc>
        [DataMember(-50)]
        [DefaultValue(null)]
        public UFile Source { get; set; }

        /// <summary>
        /// Gets or sets if source file of this asset should be copied alongside the asset file when saving.
        /// </summary>
        /// <userdoc>
        /// When saving, source file will be copied/moved alongside the asset file. On next source update, make sure to save or export to the new location when this option has just been enabled.
        /// </userdoc>
        [DataMember(-46)]
        [DefaultValue(false)]
        [Display("Keep Source Side by Side")]
        public bool SourceKeepSideBySide { get; set; }

        /// <summary>
        /// Gets or sets id of the importer used.
        /// </summary>
        /// <value>The id of the importer.</value>
        [DataMember(-40)]
        [DefaultValue(null)]
        [Browsable(false)]
        public Guid? ImporterId { get; set; }
        
        internal AssetImport GetRootBase()
        {
            if (Base != null && Base.Asset is AssetImport && Base.Id == Guid.Empty)
            {
                return (AssetImport)Base.Asset;
            }
            return null;
        }

        virtual internal void SetAsRootImport()
        {
            Id = Guid.Empty;
            Source = null;
        }
    }
}