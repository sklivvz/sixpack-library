// CacheAttributeTest.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

using System;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web;
using SixPack.ComponentModel;
using MbUnit.Framework;

namespace SixPack.UnitTests
{
    [TestFixture]
    public class CacheAttributeTest
    {
        [Test]
        public void TestExceptionsWithHttpContext()
        {
            HttpContext context = new HttpContext(new HttpRequest(null,"http://google.com",null), new HttpResponse(Console.Out));
            HttpContext.Current = context;
            CacheTestClass foo = new CacheTestClass();
            foo.resetCaching();
            Exception first = null;
            try
            {
                Console.WriteLine(String.IsNullOrEmpty(foo.ThrowExceptionWithCache()));
            }
            catch (Exception e)
            {
                first = e;
                Console.WriteLine(first.ToString());
            }

            Exception second = null;
            try
            {
                Console.WriteLine(String.IsNullOrEmpty(foo.ThrowExceptionWithoutCache()));
            }
            catch (Exception e)
            {
                second = e;
                Console.WriteLine(second.ToString());
            }

            Assert.IsNotNull(first, "First is null");
            Assert.IsNotNull(second, "Second is null");
            Assert.AreEqual(first.Message, second.Message, "First message and second message are different, but not null");
        }

        [Test]
        public void TestExceptionsWithouthHttpContext()
        {
            HttpContext.Current = null;
            CacheTestClass foo = new CacheTestClass();
            foo.resetCaching();
            Exception first = null;
            try
            {
                Console.WriteLine(String.IsNullOrEmpty(foo.ThrowExceptionWithCache()));
            }
            catch (Exception e)
            {
                first = e;
                Console.WriteLine(first.ToString());
            }

            Exception second = null;
            try
            {
                Console.WriteLine(String.IsNullOrEmpty(foo.ThrowExceptionWithoutCache()));
            }
            catch (Exception e)
            {
                second = e;
                Console.WriteLine(second.ToString());
            }

            Assert.IsNotNull(first, "First is null");
            Assert.IsNotNull(second, "Second is null");
            Assert.AreEqual(first.Message, second.Message, "First message and second message are different, but not null");
        }

        [Test]
        public void CacheAttributeTestCase()
        {
            CacheTestClass foo = new CacheTestClass();
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(foo.GetDateTime());
                Console.WriteLine(foo.GetDateTime2());
                Console.WriteLine(foo.GetDateTime3());
                DataSet ds = foo.GetDataSet();
                Console.WriteLine(ds.DataSetName);
                foreach (DataTable dt in ds.Tables)
                {
                    Console.WriteLine(dt.TableName);
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                            Console.Write("{0}: {1}\t", dc.ColumnName, dr[dc]);
                        Console.WriteLine();
                    }
                }

                ds = foo.GetDataSet("whatever");
                Console.WriteLine(ds.DataSetName);
                foreach (DataTable dt in ds.Tables)
                {
                    Console.WriteLine(dt.TableName);
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                            Console.Write("{0}: {1}\t", dc.ColumnName, dr[dc]);
                        Console.WriteLine();
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }

    [Cached]
    internal class CacheTestClass : ContextBoundObject
    {

        public void resetCaching()
        {
            Assembly a = Assembly.GetAssembly(typeof (CachedSink));
            Type t = a.GetType("SixPack.ComponentModel.CacheController");
            bool mt = (bool)t.InvokeMember("Init", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, null);
            t.GetField("isMultithreaded",BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, mt);
        }

        [CachedMethod(10)]
        public string ThrowExceptionWithCache()
        {
            exceptionThrower();
            return "You should not see me";
        }

        public string ThrowExceptionWithoutCache()
        {
            exceptionThrower();
            return "You should not see me";
        }

        private static void exceptionThrower()
        {
            throw new Exception("This is a test", new Exception("Second in stack", new Exception("Third in stack")));            
        }

        [CachedMethod(3)]
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        [CachedMethod(-1)]
        public DateTime GetDateTime2()
        {
            return DateTime.Now;
        }

        public DateTime GetDateTime3()
        {
            return DateTime.Now;
        }

        [CachedMethod(1)]
        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet("hello");
            ds.Tables.Add("world");
            ds.Tables[0].Columns.Add("bom", typeof(string));
            ds.Tables[0].Columns.Add("dia", typeof(DateTime));
            ds.Tables[0].Rows.Add(ds.Tables["world"].NewRow());
            ds.Tables[0].Rows[0][0] = "now";
            ds.Tables[0].Rows[0][1] = DateTime.Now;
            return ds;
        }

        [CachedMethod(1)]
        public DataSet GetDataSet(string whatever)
        {
            DataSet ds = new DataSet("hello");
            ds.Tables.Add("world");
            ds.Tables[0].Columns.Add("bom", typeof(string));
            ds.Tables[0].Columns.Add("dia", typeof(DateTime));
            ds.Tables[0].Rows.Add(ds.Tables["world"].NewRow());
            ds.Tables[0].Rows[0][0] = whatever;
            ds.Tables[0].Rows[0][1] = DateTime.Now;
            return ds;
        }
    }
}
