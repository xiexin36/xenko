// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;

namespace SiliconStudio.Assets.Templates
{
    /// <summary>
    /// Context that will be used to run the tempplate generator.
    /// </summary>
    public sealed class TemplateGeneratorContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGeneratorContext"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public TemplateGeneratorContext(PackageSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            Session = session;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGeneratorContext"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        public TemplateGeneratorContext(Package package)
        {
            if (package == null) throw new ArgumentNullException("package");
            if (package.Session == null) throw new ArgumentException("Package must be added to an existing PackageSession", "package");
            Package = package;
            Session = package.Session;
        }

        /// <summary>
        /// Gets or sets the current session.
        /// </summary>
        /// <value>The session.</value>
        public PackageSession Session { get; private set; }

        /// <summary>
        /// Gets or sets the current package (may be null)
        /// </summary>
        /// <value>The package.</value>
        public Package Package { get; private set; }
    }
}