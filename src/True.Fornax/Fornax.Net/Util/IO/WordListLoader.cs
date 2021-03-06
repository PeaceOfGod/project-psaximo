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
***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Fornax.Net.Util.Linq;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Util.IO
{
    /// <summary>
    /// A Loader for Plain-Text(.txt) Files that represent a list of stopwords.
    /// also  to  load stem dictionary text files.
    /// </summary>
    [Progress("WordListLoader",false, Documented = true,Tested = false)]
    public static class WordListLoader
    {
        /// <summary>
        /// Gets the word set, from a plain-text file <see cref="FileInfo"/> and adds every line as an entry to a <see cref="HashSet{T}"/>.
        /// (ommiting leading and trailing whitespaces).
        /// <para>NOTE: Every Line of the file should contain a word, if <paramref name="isBias"/> is set to true then 
        ///  all then words are preprocessed before entry. Preprocessing includes: lowercasing and removing illegal
        ///  whitespaces.
        /// </para>
        /// </summary>
        /// <param name="wordFile">The file containing words.</param>
        /// <param name="isBias">if set to <c>true</c> words are copied without any preprocessing.</param>
        /// <returns>A set of each distinct word from file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HashSet<string> GetWordSet(FileInfo wordFile, bool isBias) {
            Contract.Requires(wordFile != null);
            if (wordFile == null) throw new ArgumentNullException();

            HashSet<string> result = new HashSet<string>();
            using (var reader = new StreamReader(wordFile.FullName, Encoding.Default, true)) {
                result = GetWordSet(reader, isBias);
            }
            return result;
        }

        /// <summary>
        /// Gets the word set, from a Reader <see cref="TextReader"/> and adds every line as an entry to a <see cref="HashSet{T}"/>.
        /// (ommiting leading and trailing whitespaces).
        /// <para>NOTE: Every Line of the file should contain a word, if <paramref name="isBias"/> is set to true then 
        ///  all then words are preprocessed before entry. Preprocessing includes: lowercasing and removing illegal
        ///  whitespaces.
        /// </para>
        /// </summary>
        /// <param name="wordFile">The file containing words.</param>
        /// <param name="isBias">if set to <c>true</c> words are copied without any preprocessing.</param>
        /// <returns>A set of each distinct word from file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HashSet<string> GetWordSet(TextReader reader, bool isBias) {
            Contract.Requires(reader != null);
            if (reader == null) throw new ArgumentNullException($"{nameof(reader)} is null.");
            HashSet<string> result = new HashSet<string>();

            try {
                string word = null;
                while ((word = reader.ReadLine()) != null) {
                    if (isBias)
                        result.Add(word.Trim());
                    else result.Add(word.ToLower().Clean(false));
                }
            }
            finally { reader.Close(); }
            return result;
        }

        /// <summary>
        /// Gets the word set asynchronuously, from a plain-text file <see cref="FileInfo"/> and adds every line as an entry to a <see cref="HashSet{T}"/>.
        /// (ommiting leading and trailing whitespaces).
        /// <para>NOTE: Every Line of the file should contain a word, if <paramref name="isBias"/> is set to true then 
        ///  all then words are preprocessed before entry. Preprocessing includes: lowercasing and removing illegal
        ///  whitespaces.
        /// </para>
        /// </summary>
        /// <param name="wordFile">The file containing words.</param>
        /// <param name="isBias">if set to <c>true</c> words are copied without any preprocessing.</param>
        /// <returns>A set of each distinct word from file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<HashSet<string>> GetWordSetAsync(FileInfo wordfile, bool isBias) {
            Contract.Requires(wordfile != null);
            if (wordfile == null) throw new ArgumentNullException($"{nameof(wordfile)} is null.");

            HashSet<string> result = new HashSet<string>();
            using (var reader = new StreamReader(wordfile.FullName, Encoding.Default, true)) {
                result = await GetWordSetAsync(reader, isBias);
            }
            return result;
        }

        /// <summary>
        /// Gets the word set asynchronuously, from a Reader <see cref="TextReader"/> and adds every line as an entry to a <see cref="HashSet{T}"/>.
        /// (ommiting leading and trailing whitespaces).
        /// <para>NOTE: Every Line of the file should contain a word, if <paramref name="isBias"/> is set to true then 
        ///  all then words are preprocessed before entry. Preprocessing includes: lowercasing and removing illegal
        ///  whitespaces.
        /// </para>
        /// </summary>
        /// <param name="wordFile">The file containing words.</param>
        /// <param name="isBias">if set to <c>true</c> words are copied without any preprocessing.</param>
        /// <returns>A set of each distinct word from file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<HashSet<string>> GetWordSetAsync(TextReader reader, bool isBias) {
            Contract.Requires(reader != null);
            if (reader == null) throw new ArgumentNullException($"{nameof(reader)} is null.");
            HashSet<string> result = new HashSet<string>();
            try {
                string word = null;
                while ((word = await reader.ReadLineAsync()) != null) {
                    if (isBias) result.Add(word);
                    else result.Add(word.ToLower().Clean(false));
                }
            }
            finally { reader.Close(); }
            return result;
        }

        /// <summary>
        /// Updates the specified <see cref="ICollection{T}"/> word list. From a specific word File.
        /// as in <seealso cref="GetWordSet(FileInfo, bool)"/>.
        /// </summary>
        /// <param name="wordList">The current word list.</param>
        /// <param name="wordsFile">The words file containing new words.</param>
        /// <param name="isBias">if set to <c>true</c> Text Prepocessing is enabled.</param>
        /// <param name="AsSet">if set to <c>true</c> no repeated words allowed.</param>
        public static void Update(ref ICollection<string> wordList, FileInfo wordsFile, bool isBias) {
            Contract.Requires(wordList != null || wordsFile != null);
            try {
                using (var reader = new StreamReader(wordsFile.FullName, Encoding.Default, true)) {
                    Update(ref wordList, reader, isBias);
                }
            } catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is IOException) { }
        }

        /// <summary>
        /// Updates the specified <see cref="ICollection{T}"/> word list. by/from a specific Reader
        /// as in <seealso cref="GetWordSet(FileInfo, bool)"/>.
        /// </summary>
        /// <param name="wordList">The current word list.</param>
        /// <param name="wordsFile">The words file containing new words.</param>
        /// <param name="isBias">if set to <c>true</c> Text Prepocessing is enabled.</param>
        /// <param name="AsSet">if set to <c>true</c> no repeated words allowed.</param>
        public static void Update(ref ICollection<string> wordList, TextReader textReader, bool isBias) {
            Contract.Requires(wordList != null || textReader != null);
            try {
                ICollection<string> temp = new HashSet<string>();
      
                string word = null;
                while ((word = textReader.ReadLine()) != null) {
                    if (isBias)
                        temp.Add(word.Trim());
                    else temp.Add(word.ToLower().Clean(false));
                }
                wordList.AddAll(temp);
            } catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is IOException) { }
        }

        /// <summary>
        /// Updates the specified <see cref="ICollection{T}"/> word list asynchronously. From a specific word File.
        /// as in <seealso cref="GetWordSet(FileInfo, bool)"/>.
        /// </summary>
        /// <param name="wordList">The current word list.</param>
        /// <param name="wordsFile">The words file containing new words.</param>
        /// <param name="isBias">if set to <c>true</c> Text Prepocessing is enabled.</param>
        /// <param name="AsSet">if set to <c>true</c> no repeated words allowed.</param>
        public static async Task<ICollection<string>> UpdateAsync(ICollection<string> wordList, FileInfo wordsFile, bool isBias) {
            Contract.Requires(wordList != null || wordsFile != null);
            try {
                using (var reader = new StreamReader(wordsFile.FullName, Encoding.Default, true)) {
                    return await UpdateAsync(wordList, reader, isBias);
                }
            } catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is IOException) { return wordList; }

        }

        /// <summary>
        /// Updates the specified <see cref="ICollection{T}"/> word list asynchronuously. by/from a specific Reader
        /// as in <seealso cref="GetWordSet(FileInfo, bool)"/>.
        /// </summary>
        /// <param name="wordList">The current word list.</param>
        /// <param name="wordsFile">The words file containing new words.</param>
        /// <param name="isBias">if set to <c>true</c> Text Prepocessing is enabled.</param>
        /// <param name="AsSet">if set to <c>true</c> no repeated words allowed.</param>
        internal static async Task<ICollection<string>> UpdateAsync(ICollection<string> wordList, TextReader reader, bool isBias) {
            Contract.Requires(wordList != null || reader != null);
            try {
                ICollection<string> temp = new HashSet<string>();

                string word = null;
                while ((word = await reader.ReadLineAsync()) != null) {
                    if (isBias)
                        temp.Add(word.Trim());
                    else temp.Add(word.ToLower().Clean(false));
                }
                wordList.AddAll(temp);
                return wordList;
            } catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is IOException) { return wordList; }
        }

        //TODO: Stem dictionary loader , stop words with comment loader(rep comment tag e.g "|")

        //public static IDictionary<string, HashSet<string>> GetStemDictionary(FileInfo file, bool IsBias) {


        //}

    }
}
