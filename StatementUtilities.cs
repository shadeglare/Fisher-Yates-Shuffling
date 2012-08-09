using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Shadeglare.Helpers
{
   /// <summary>
   /// Contains methods for parsing and collecting
   /// tokens informations in string statements
   /// </summary>
   public static class StatementUtilities
   {
      /// <summary>
      /// The array of Latin Vowels Symbols
      /// </summary>
      private static Char[] LatinVowels = 
      {
          'a', 'e', 'i', 'o', 'u', 'y'
      };

      /// <summary>
      /// The array of default word separators
      /// </summary>
      private static Char[] DefaultSeparators =
      {
          ' '
      };

      /// <summary>
      /// Collects words information from a given statement.
      /// </summary>
      /// <param name="statement">An array of characters for parsing.</param>
      /// <returns>Returs a list of WordInfo objects.</returns>
      public static List<WordInfo> CollectWordsInfo(Char[] statement)
      {
         return CollectWordsInfo(statement, false);
      }

      /// <summary>
      /// Collects words information from a given statement.
      /// </summary>
      /// <param name="statement">An array of characters for the parsing.</param>
      /// <param name="collectVowelsInfo">Specifies needed in collection of latin vowels.</param>
      /// <returns>Returs a list of WordInfo objects.</returns>
      /// <remarks>
      /// Informations about vowel positions in words always sorted by ascendant.
      /// </remarks>
      public static List<WordInfo> CollectWordsInfo(Char[] statement, Boolean collectVowelsInfo)
      {
         return CollectWordsInfo(statement, collectVowelsInfo, ' ');
      }

      /// <summary>
      /// Collects words information from a given statement.
      /// </summary>
      /// <param name="statement">An array of characters for the parsing.</param>
      /// <param name="collectVowelsInfo">Specifies needed in collection of latin vowels.</param>
      /// <param name="separators">Word separators.</param>
      /// <returns>Returs a list of WordInfo objects.</returns>
      /// <remarks>
      /// Informations about vowel positions in words always sorted by ascendant.
      /// </remarks>
      public static List<WordInfo> CollectWordsInfo(
         Char[] statement, 
         Boolean collectVowelsInfo, 
         params Char[] separators
         )
      {
         var wordsInfo = new List<WordInfo>();
         WordInfo tempInfo;
         Int32 begPosition = 0;
         Int32 endPosition = 0;

         while (true)
         {
            begPosition = findWordBeginPosition(statement, begPosition, separators);
            if (begPosition == -1) { break; };
            endPosition = findWordEndPosition(statement, begPosition, separators);
            tempInfo = new WordInfo(begPosition, endPosition);
            if (collectVowelsInfo) { collectLatinVowelsInfo(statement, ref tempInfo); }
            
            wordsInfo.Add(tempInfo);
            begPosition = endPosition + 1;
         }

         return wordsInfo;
      }

      /// <summary>
      /// Returns a position of a word start.
      /// </summary>
      /// <param name="statement">An array of characters for the parsing.</param>
      /// <param name="startIndex">An index from where to search.</param>
      /// <param name="separators">Word separators.</param>
      /// <returns>An index of the first letter of a word is found.</returns>
      /// <remarks>
      /// This method can be used in a combination with FindWordEndPosition for an obtaining word bounds.
      /// </remarks>
      private static Int32 findWordBeginPosition(Char[] statement, Int32 startIndex, params Char[] separators)
      {
         Int32 position = -1;
         Char[] currentSeparators = null;

         if (separators == null || separators.Length == 0)
         {
            currentSeparators = DefaultSeparators;
         }
         else
         {
            currentSeparators = separators;
         }

         while (startIndex < statement.Length)
         {
            if (!currentSeparators.Contains(statement[startIndex]))
            {
               position = startIndex;
               break;
            }
            startIndex++;
         }

         return position;
      }

      /// <summary>
      /// Returns a position of a word end.
      /// </summary>
      /// <param name="statement">An array of characters for the parsing.</param>
      /// <param name="startIndex">An index from where to search.</param>
      /// <param name="separators">Word separators.</param>
      /// <returns>An index of the last letter of a word to found.</returns>
      /// <remarks>
      /// This method can be used in a combination with FindWordBeginPosition for an obtaining word bounds.
      /// </remarks>
      private static Int32 findWordEndPosition(Char[] statement, Int32 startIndex, params Char[] separators)
      {
         Int32 position = startIndex;
         Char[] currentSeparators = null;

         if (separators == null || separators.Length == 0)
         {
            currentSeparators = DefaultSeparators;
         }
         else
         {
            currentSeparators = separators;
         }

         while (startIndex < statement.Length)
         {

            if (currentSeparators.Contains(statement[startIndex]))
            {
               break;
            }
            startIndex++;
         }

         position = startIndex - 1;
         return position;
      }

      /// <summary>
      /// Collects latin vowels postions of word in a statement. Word bounds info is contained in a wordInfo argument.
      /// </summary>
      /// <param name="statement">An array of characters for the parsing.</param>
      /// <param name="wordInfo">An instance of WordInfo class provided word bounds info and it's
      /// where latin vowels positions would be added.
      /// </param>
      private static void collectLatinVowelsInfo(Char[] statement, ref WordInfo wordInfo)
      {
         for (Int32 i = wordInfo.BeginPosition; i <= wordInfo.EndPosition; i++)
         {
            if (LatinVowels.Contains(statement[i]))
            {
               wordInfo.VowelPositions.Add(i);
            }
         }
      }

      /// <summary>
      /// Shuffles letters in words contained in a statement.
      /// </summary>
      /// <param name="statment">A string for a shuffle.</param>
      /// <returns>A shuffle string.</returns>
      public static String ShuffleStatement(String statment)
      {
         return ShuffleStatement(statment, false);
      }

      /// <summary>
      /// Shuffles letters in words contained in a statement.
      /// </summary>
      /// <param name="statement">A string for a shuffle.</param>
      /// <param name="shuffleOnlyVowels">Specifies when only vowels shuffle is needed.</param>
      /// <returns>A shuffle string.</returns>
      public static String ShuffleStatement(String statement, Boolean shuffleOnlyVowels)
      {
         return ShuffleStatement(statement, shuffleOnlyVowels, null);
      }

      /// <summary>
      /// Shuffles letters in words contained in a statement.
      /// </summary>
      /// <param name="statement">A string for a shuffle.</param>
      /// <param name="shuffleOnlyVowels">Specifies when only vowels shuffle is needed.</param>
      /// <param name="separators">User defined words separators.</param>
      /// <returns>A shuffle string.</returns>
      public static String ShuffleStatement(
         String statement, 
         Boolean shuffleOnlyVowels,
         params Char[] separators
         )
      {
         Char[] letters = statement.ToCharArray();
         var wordInfos = CollectWordsInfo(letters, shuffleOnlyVowels, separators);

         if (shuffleOnlyVowels)
         {
            foreach (var info in wordInfos)
            {
               letters.Shuffle(info.BeginPosition, info.EndPosition, info.VowelPositions);
            }
         }
         else
         {
            foreach (var info in wordInfos)
            {
               letters.Shuffle(info.BeginPosition, info.EndPosition);
            }
         }
         
         return new String(letters);
      }

      /// <summary>
      /// Shuffle elements values of a XML document.
      /// </summary>
      /// <param name="xmlDocument">A XElement object for a shuffle.</param>
      public static void ShuffleXmlElementsValues(XDocument xmlDocument)
      {
         ShuffleXmlElementsValues(xmlDocument, false);
      }

      /// <summary>
      /// Shuffle elements values of a XML document.
      /// </summary>
      /// <param name="xmlDocument">A XElement object for a shuffle.</param>
      /// <param name="shuffleOnlyVowels">Specifies when only vowels shuffle is needed.</param>
      public static void ShuffleXmlElementsValues(XDocument xmlDocument, Boolean shuffleOnlyVowels)
      {
         ShuffleXmlElementsValues(xmlDocument, shuffleOnlyVowels, null);
      }

      /// <summary>
      /// Shuffle elements values of a XML document.
      /// </summary>
      /// <param name="xmlDocument">A XElement object for a shuffle.</param>
      /// <param name="shuffleOnlyVowels">Specifies when only vowels shuffle is needed.</param>
      /// <param name="separators">User defined words separators in xml elements values.</param>
      public static void ShuffleXmlElementsValues(
         XDocument xmlDocument, 
         Boolean shuffleOnlyVowels, 
         params Char[] separators
         )
      {
         var textNodes = from node in xmlDocument.DescendantNodes()
                         where node is XText
                         select (XText)node;
         foreach (var textNode in textNodes)
         {
            string tempValue = textNode.Value;
            tempValue = ShuffleStatement(tempValue, shuffleOnlyVowels, separators);
            textNode.Value = tempValue;
         }

      }
   }
}