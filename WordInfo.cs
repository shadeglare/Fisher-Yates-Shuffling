using System;
using System.Collections.Generic;

namespace Shadeglare.Helpers
{
   /// <summary>
   /// Describes a word information in a string.
   /// </summary>
   public struct WordInfo
   {
      private Int32 m_beginPosition;
      private Int32 m_endPosition;
      private List<Int32> m_vowelPositions;

      /// <summary>
      /// Initializes a new instance of WordInfo class.
      /// </summary>
      public WordInfo(Int32 beginPosition, Int32 endPosition)
      {
         m_beginPosition = beginPosition;
         m_endPosition = endPosition;
         m_vowelPositions = new List<int>();
      }

      /// <summary>
      /// Gets or sets a begin position of a word.
      /// </summary>
      public Int32 BeginPosition
      {
         get { return m_beginPosition; }
         set { m_beginPosition = value; }
      }

      /// <summary>
      /// Gets or sets an end position of a word.
      /// </summary>
      public Int32 EndPosition
      {
         get { return m_endPosition; }
         set { m_endPosition = value; }
      }

      /// <summary>
      /// Gets vowel positions in a word.
      /// </summary>
      public List<Int32> VowelPositions
      {
         get { return m_vowelPositions; }
      }
   }
}