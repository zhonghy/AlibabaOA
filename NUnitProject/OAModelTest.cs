using AlibabaOA.DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Common;
using System.Data.SqlClient;

namespace NUnitProject
{
    [TestFixture]
    public class OAModelTest : IDisposable
    {
        [SetUp]
        public static void Init()
        {
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }

        [Test]
        public static void InsertProduct()
        {
            using (var context = new ProductEFContext())
            {
                var product = new Product
                {
                    SKU = 147,
                    Description = "Expandable Hydration Pack",
                    Price = 19.97M,
                    ImageURL = "/pack147.jpg"
                };
                context.Products.Add(product);

                product = new Product
                {
                    SKU = 178,
                    Description = "Rugged Ranger Duffel Bag",
                    Price = 39.97M,
                    ImageURL = "/pack178.jpg"
                };
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        [Test]
        public static void ReadProduct()
        {
            using (var context = new ProductEFContext())
            {
                foreach (var p in context.Products.Take(10))
                {
                    Console.WriteLine("{0} {1} {2} {3}", p.SKU, p.Description,
                    p.Price.ToString("C"), p.ImageURL);
                }
            }
        }

        [Test]
        public static void InsertBusiness()
        {
            using (var context = new BusinessContext())
            {
                var business = new Business
                {
                    Name = "Corner Dry Cleaning",
                    LicenseNumber = "100x1"
                };
                context.Businesses.Add(business);

                var retail = new Retail
                {
                    Name = "Shop and Save",
                    LicenseNumber = "200C",
                    Address = "101 Main",
                    City = "Anytown",
                    State = "TX",
                    ZIPCode = "76106"
                };
                context.Businesses.Add(retail);

                var web = new eCommerce
                {
                    Name = "BuyNow.com",
                    LicenseNumber = "300AB",
                    URL = "www.buynow.com"
                };
                context.Businesses.Add(web);

                context.SaveChanges();
            }
        }

        [Test]
        public static void ReadBusiness()
        {
            using (var context = new BusinessContext())
            {
                Console.WriteLine("\n--- All Businesses ---");
                foreach (var b in context.Businesses)
                {
                    Console.WriteLine("{0} (#{1})", b.Name, b.LicenseNumber);
                }

                Console.WriteLine("\n--- Retail Businesses ---");
                foreach (var r in context.Businesses.OfType<Retail>())
                {
                    Console.WriteLine("{0} (#{1})", r.Name, r.LicenseNumber);
                    Console.WriteLine("{0}", r.Address);
                    Console.WriteLine("{0}, {1} {2}", r.City, r.State, r.ZIPCode);
                }

                Console.WriteLine("\n--- eCommerce Businesses ---");
                foreach (var e in context.Businesses.OfType<eCommerce>())
                {
                    Console.WriteLine("{0} (#{1})", e.Name, e.LicenseNumber);
                    Console.WriteLine("Online address is: {0}", e.URL);
                }
            }
        }

        [Test]
        public static void InsertLocation()
        {
            using (var context = new LocationEntities())
            {
                var park = new Park
                {
                    Name = "11th Street Park",
                    Address = "801 11th Street",
                    City = "Aledo",
                    State = "TX",
                    ZIPCode = "76106"
                };
                var loc = new Location
                {
                    Address = "501 Main",
                    City = "Weatherford",
                    State = "TX",
                    ZIPCode = "76201"
                };
                park.Office = loc;
                context.Locations.Add(park);
                context.SaveChanges();
            }
        }

        [Test]
        public static void InsertAgent()
        {
            using (var context = new AgentEntities())
            {
                var name1 = new Name { FirstName = "Robin", LastName = "Rosen" };
                var name2 = new Name { FirstName = "Alex", LastName = "St. James" };

                var address1 = new Address
                {
                    AddressLine1 = "510 N. Grant",
                    AddressLine2 = "Apt. 8",
                    City = "Raytown",
                    State = "MO",
                    ZIPCode = "64133"
                };
                var address2 = new Address
                {
                    AddressLine1 = "222 Baker St.",
                    AddressLine2 = "Apt.22B",
                    City = "Raytown",
                    State = "MO",
                    ZIPCode = "64133"
                };

                context.Agents.Add(new Agent() { Name = name1, Address = address1 });
                context.Agents.Add(new Agent() { Name = name2, Address = address2 });

                context.SaveChanges();
            }
        }

        [Test]
        public static void RunForEachAsyncExample()
        {
            using (var context = new AssociateEntities())
            {
                context.Associates.Include(x => x.AssociateSalaries).ForEachAsync(m =>
                {
                    Console.WriteLine("Here are the salaries for Associate {0}:", m.Name);
                    foreach (var salary in m.AssociateSalaries)
                    {
                        Console.WriteLine("\t{0}", salary.Salary);
                    }
                });
            }
        }

        [Test]
        public static void RunToListAsyncExample()
        {
            using (var context = new AssociateEntities())
            {
                //借助ToListAsyn方法
                var associates = context.Associates.Include(x => x.AssociateSalaries).OrderBy(x => x.Name).ToList();
                foreach (var associate in associates)
                {
                    Console.WriteLine("Here are the salaries for Associate {0}:", associate.Name);
                    foreach (var salaryInfo in associate.AssociateSalaries)
                    {
                        Console.WriteLine("\t{0}", salaryInfo.Salary);
                    }
                }
            }
        }

        [Test]
        public static void StudentEFTest()
        {
            using (var context = new StudentEntities())
            {
                //删除测试数据
                context.Database.ExecuteSqlCommand("delete from chapter3.student");

                //添加新数据
                context.Students.Add(new Student
                {
                    FirstName = "Robert",
                    LastName = "Smith",
                    Degree = "Masters"
                });

                context.Students.Add(new Student
                {
                    FirstName = "Julia",
                    LastName = "Kerns",
                    Degree = "Masters"
                });

                context.Students.Add(new Student
                {
                    FirstName = "Nancy",
                    LastName = "Stiles",
                    Degree = "Doctorate"
                });

                context.SaveChanges();
            }

            //读取数据
            using (var context = new StudentEntities())
            {
                var sql = "select * from Chapter3.Student where Degree = @Major";
                var parameters = new DbParameter[]
                {
                    new SqlParameter{ParameterName = "Major", Value = "Masters"}
                };

                var students = context.Database.SqlQuery<Student>(sql, parameters);
                foreach (var stu in students)
                {
                    Console.WriteLine("{0} {1} is working on a {2} degree", stu.FirstName, stu.LastName, stu.Degree);
                }
            }
        }

        [Test]
        public static void CustomerEFPageTest()
        {
            //using (var context = new CustomerEntities())
            //{
            //    // 删除之前的数据
            //    context.Database.ExecuteSqlCommand("delete from Chapter3.Customer");

            //    // 添加新的测试数据
            //    context.Customers.Add(new Customer
            //    {
            //        Name = "Roberts, Jill",
            //        Email = "jroberts@abc.com"
            //    });
            //    context.Customers.Add(new Customer
            //    {
            //        Name = "Robertson, Alice",
            //        Email = "arob@gmail.com"
            //    });
            //    context.Customers.Add(new Customer
            //    {
            //        Name = "Rogers, Steven",
            //        Email = "srogers@termite.com"
            //    });
            //    context.Customers.Add(new Customer
            //    {
            //        Name = "Roe, Allen",
            //        Email = "allenr@umc.com"
            //    });
            //    context.Customers.Add(new Customer
            //    {
            //        Name = "Jones, Chris",
            //        Email = "cjones@ibp.com"
            //    });
            //    context.SaveChanges();
            //}

            using (var context = new CustomerEntities())
            {
                string match = "Ro";
                int pageIndex = 1;
                int pageSize = 3;

                var customers = context.Customers.Where(m => m.Name.Contains(match))
                    .OrderBy(m => m.Name)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize);
                Console.WriteLine("Customers Ro*");
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0} [email: {1}]", customer.Name, customer.Email);
                }
            }
        }

        [Test]
        public static void AccountOrderEFTest()
        {
            using (var context = new AccountEntities())
            {
                //删除之前的测试数据
                context.Database.ExecuteSqlCommand("delete from chapter3.[order]");
                context.Database.ExecuteSqlCommand("delete from chapter3.account");

                //添加新的测试数据
                var account1 = new Account { City = "Raytown", State = "MO" };
                account1.Orders.Add(new Order
                {
                    Amount = 223.09M,
                    ShipCity = "Raytown",
                    ShipState = "MO"
                });
                account1.Orders.Add(new Order
                {
                    Amount = 189.32M,
                    ShipCity = "Olathe",
                    ShipState = "KS"
                });

                var account2 = new Account { City = "Kansas City", State = "MO" };
                account2.Orders.Add(new Order
                {
                    Amount = 99.29M,
                    ShipCity = "Kansas City",
                    ShipState = "MO"
                });

                var account3 = new Account { City = "North Kansas City", State = "MO" };
                account3.Orders.Add(new Order
                {
                    Amount = 102.29M,
                    ShipCity = "Overland Park",
                    ShipState = "KS"
                });

                context.Accounts.Add(account1);
                context.Accounts.Add(account2);
                context.Accounts.Add(account3);
                context.SaveChanges();
            }

            using (var context = new AccountEntities())
            {
                var orders = from o in context.Orders
                             join a in context.Accounts on
                             new { Id = o.AccountId, City = o.ShipCity, State = o.ShipState }
                             equals
                             new { Id = a.AccountId, City = a.City, State = a.State }
                             select o;
                Console.WriteLine("Orders shipped to the account's city, state...");
                foreach (var order in orders)
                {
                    Console.WriteLine("\tOrder {0} for {1}", order.AccountId.ToString(), order.Amount.ToString());
                }
            }
        }

        public void Dispose()
        {
            //
        }
    }
}
