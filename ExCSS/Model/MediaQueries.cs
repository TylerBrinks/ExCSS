using System;
using System.Collections;
using System.Collections.Generic;

namespace ExCSS.Model
{
    public sealed class MediaQueries : IEnumerable<string>
    {
        private readonly List<string> _media;
        private string _buffer;

        internal MediaQueries()
        {
            _buffer = string.Empty;
            _media = new List<string>();
        }


        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _media.Count)
                {
                    return null;
                }

                return _media[index];
            }
        }

        public int Length
        {
            get { return _media.Count; }
        }

        public string MediaText
        {
            get { return _buffer; }
            set
            {
                _buffer = string.Empty;
                _media.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    var entries = value.SplitCommas();

                    for (var i = 0; i < entries.Length; i++)
                    {
                        var a = 1;
                        //if (!CheckSyntax(entries[i]))
                        //    throw new DOMException(ErrorCode.SyntaxError);
                    }

                    for (var i = 0; i < entries.Length; i++)
                    {
                        AppendMedium(entries[i]);
                    }
                }
            }
        }

        public MediaQueries AppendMedium(string newMedium)
        {
            if (!CheckSyntax(newMedium))
            {
                var a = 1;
                //throw new DOMException(ErrorCode.SyntaxError);
            }

            if (!_media.Contains(newMedium))
            {
                _media.Add(newMedium);
                _buffer += (String.IsNullOrEmpty(_buffer) ? string.Empty : ",") + newMedium;
            }

            return this;
        }

        public MediaQueries DeleteMedium(string oldMedium)
        {
            if (!_media.Contains(oldMedium))
            {
                var a = 1;
                //throw new DOMException(ErrorCode.ItemNotFound);
            }

            _media.Remove(oldMedium);

            if (_buffer.StartsWith(oldMedium))
            {
                _buffer.Remove(0, oldMedium.Length + 1);
            }
            else
            {
                _buffer.Replace("," + oldMedium, string.Empty);
            }

            return this;
        }


        private static bool CheckSyntax(string medium)
        {
            return !string.IsNullOrEmpty(medium);
        }

        
        public IEnumerator<String> GetEnumerator()
        {
            return ((IEnumerable<string>) _media).GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}