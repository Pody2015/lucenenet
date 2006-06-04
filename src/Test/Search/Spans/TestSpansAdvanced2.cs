/*
 * Copyright 2005 The Apache Software Foundation
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using NUnit.Framework;
using StandardAnalyzer = Lucene.Net.Analysis.Standard.StandardAnalyzer;
using IndexReader = Lucene.Net.Index.IndexReader;
using IndexWriter = Lucene.Net.Index.IndexWriter;
using Term = Lucene.Net.Index.Term;
using BooleanClause = Lucene.Net.Search.BooleanClause;
using BooleanQuery = Lucene.Net.Search.BooleanQuery;
using Hits = Lucene.Net.Search.Hits;
using Query = Lucene.Net.Search.Query;

namespace Lucene.Net.Search.Spans
{
	
	/// <summary>****************************************************************************
	/// Some expanded tests to make sure my patch doesn't break other SpanTermQuery
	/// functionality.
	/// 
	/// </summary>
	/// <author>  Reece Wilton
	/// </author>
	public class TestSpansAdvanced2:TestSpansAdvanced
	{
		
		/// <summary> Initializes the tests by adding documents to the index.</summary>
		[TestFixtureSetUp]
        public virtual void  SetUp()
		{
			base.SetUp();
			
			// create test index
			IndexWriter writer = new IndexWriter(mDirectory, new StandardAnalyzer(), false);
			AddDocument(writer, "A", "Should we, could we, would we?");
			AddDocument(writer, "B", "It should.  Should it?");
			AddDocument(writer, "C", "It shouldn't.");
			AddDocument(writer, "D", "Should we, should we, should we.");
			writer.Close();
		}
		
		/// <summary> Verifies that the index has the correct number of documents.
		/// 
		/// </summary>
		/// <throws>  Exception </throws>
		[Test]
        public virtual void  TestVerifyIndex()
		{
			IndexReader reader = IndexReader.Open(mDirectory);
			Assert.AreEqual(8, reader.NumDocs());
			reader.Close();
		}
		
		/// <summary> Tests a single span query that matches multiple documents.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		[Test]
        public virtual void  TestSingleSpanQuery()
		{
			
			Query spanQuery = new SpanTermQuery(new Term(FIELD_TEXT, "should"));
			Hits hits = ExecuteQuery(spanQuery);
			System.String[] expectedIds = new System.String[]{"B", "D", "1", "2", "3", "4", "A"};
			float[] expectedScores = new float[]{0.625f, 0.45927936f, 0.35355338f, 0.35355338f, 0.35355338f, 0.35355338f, 0.26516503f};
			AssertHits(hits, "single span query", expectedIds, expectedScores);
		}
		
		/// <summary> Tests a single span query that matches multiple documents.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		[Test]
        public virtual void  TestMultipleDifferentSpanQueries()
		{
			
			Query spanQuery1 = new SpanTermQuery(new Term(FIELD_TEXT, "should"));
			Query spanQuery2 = new SpanTermQuery(new Term(FIELD_TEXT, "we"));
			BooleanQuery query = new BooleanQuery();
			query.Add(spanQuery1, BooleanClause.Occur.MUST);
			query.Add(spanQuery2, BooleanClause.Occur.MUST);
			Hits hits = ExecuteQuery(query);
			System.String[] expectedIds = new System.String[]{"A", "D"};
			float[] expectedScores = new float[]{0.93163157f, 0.20698164f};
			AssertHits(hits, "multiple different span queries", expectedIds, expectedScores);
		}
		
		/// <summary> Tests two span queries.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		[Test]
        public override void  TestBooleanQueryWithSpanQueries()
		{
			
			DoTestBooleanQueryWithSpanQueries(0.73500174f);
		}
	}
}