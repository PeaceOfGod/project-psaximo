﻿/** 
* Copyright (c) 2017 Koudura Ninci @True.Inc
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*
**/

namespace Fornax.Net.Index
{
    #region Index Enumerations

    /// <summary>
    /// Indexing strategy to be used by fornax.
    /// </summary>
    public enum IndexMode
    {

        /// <summary>
        /// The per cache indexing mode.
        /// </summary>
        PerCache,

        /// <summary>
        /// The per file indexing mode.
        /// </summary>
        PerFile,

        /// <summary>
        /// The per memory indexing mode.
        /// </summary>
        PerMemory,

        /// <summary>
        /// The per segment indexing mode.
        /// </summary>
        PerSegment

    }

    /// <summary>
    ///  <see cref="FieldScope"/> defines a fieldbased query. 
    ///  Scope to field implementation used here.
    ///  Field Scope is an attribute or property of a file that can be extracted or retreived by fornax.net.
    /// </summary>
    public enum FieldScope : sbyte
    {
        /// <summary>
        /// The content of document.
        /// </summary>
        Content = 0x0000,

        /// <summary>
        /// The date created of document.
        /// </summary>
        Date = 0x0001,

        /// <summary>
        /// The date last modified.
        /// </summary>
        Modified = 0x0002,

        /// <summary>
        /// The name or/and title of the document.
        /// </summary>
        Title = 0x0003,

        /// <summary>
        /// The path or link to doument in directory.
        /// </summary>
        Path = 0x0004,

        /// <summary>
        /// The file format of document.
        /// </summary>
        Type = 0x0005,

        /// <summary>
        /// The meta data
        /// </summary>
        MetaData = 0x0006

    }

    /// <summary>
    /// Zone is a Searchable <see cref="FieldScope"/>
    /// </summary>
    public enum Zone : byte
    {
        /// <summary>
        /// The content. see <seealso cref="FieldScope.Content"/>
        /// </summary>
        Content = 0b1111,
        /// <summary>
        /// The metadata see <seealso cref="FieldScope.MetaData"/>
        /// </summary>
        Metadata = 0b1110,
        /// <summary>
        /// The title or name of the document. see <seealso cref="FieldScope.Title"/>
        /// </summary>
        Title = 0b1100,
        /// <summary>
        /// The facet, which is a segment of any searchable fieldscope.
        /// </summary>
        Facet = 0b1000,
        /// <summary>
        /// The custom Zone which is to be defined by user.
        /// </summary>
        Custom = 0b0001

    }
    #endregion
}
