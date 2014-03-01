﻿using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace Twitta.Website.Models
{
    public class TweetProcessor
    {
        private readonly List<string> _stopwords =
            "&amp;,a,able,about,across,after,all,almost,also,am,among,an,and,any,are,as,at,be,because,been,but,by,can,cannot,could,dear,did,do,does,either,else,ever,every,for,from,get,got,had,has,have,he,her,hers,him,his,how,however,i,if,in,into,is,it,its,just,least,let,like,likely,may,me,might,most,must,my,neither,no,nor,not,of,off,often,on,only,or,other,our,own,rather,said,say,says,she,should,since,so,some,than,that,the,their,them,then,there,these,they,this,tis,to,too,twas,us,wants,was,we,were,what,when,where,which,while,who,whom,why,will,with,would,yet,you,your"
                .Split(',').ToList();

        public Dictionary<string, int> WordCountStats(List<string> tweets)
        {
            //every word in all tweets found
            string txt = tweets.Aggregate(string.Empty, (current, t) => current + t);

            //Convert the string into an array of words spliting on comma and space
            string[] source = txt.ToLowerInvariant().Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

            //get a distinct list of those words
            var distinctWords = new List<string>(source.Distinct());

            //make a dictionary of word and usage frequency
            var ignoreWordLogic = ObjectFactory.GetInstance<IIgnoreWordLogic>();
            _stopwords.AddRange(ignoreWordLogic.GetItems().Select(w => w.WordText).ToList());
            return distinctWords.Except(_stopwords).ToDictionary(word => word, word => Wordcount(word, source));
        }

        private int Wordcount(string searchTerm, string[] wordlist)
        {
            // Create and execute the query. It executes immediately  
            // because a singleton value is produced. 
            // Use ToLowerInvariant to match "data" and "Data"  
            var matchQuery = from word in wordlist
                where word == searchTerm
                select word;

            // Count the matches. 
            return matchQuery.Count();
        }
    }
}