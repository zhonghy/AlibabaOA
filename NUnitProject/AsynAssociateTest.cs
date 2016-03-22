using AlibabaOA.DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NUnitProject
{
    [TestFixture]
    public class AsynAssociateTest
    {
        [Test]
        public static void StartAsynTest()
        {
            var asyn = AssociateAsynDemo();
            foreach (var c in BusyChars())
            {
                if (asyn.IsCompleted)
                {
                    break;
                }

                Console.Write(c);
                Console.CursorLeft = 0;
                Thread.Sleep(100);
            }

            Console.WriteLine("\n所有异步操作已经完成!!");
        }

        private static IEnumerable<char> BusyChars()
        {
            while (true)
            {
                yield return '\\';
                yield return '|';
                yield return '/';
                yield return '-';
            }
        }

        private static async Task AssociateAsynDemo()
        {
            await CleanUp();
            await LoadData();
            await RunForEachAsyncExample();
            await RunToListAsyncExample();
            await RunSingleOrDefaultAsyncExample();
        }

        private static async Task CleanUp()
        {
            using (var context = new AssociateEntities())
            {
                //清除原始数据
                //异步执行原始SQL语句
                Console.WriteLine("Cleaning Up Previous Test Data");
                Console.WriteLine("=========\n");

                await context.Database.ExecuteSqlCommandAsync("delete from Chapter3.AssociateSalary");
                await context.Database.ExecuteSqlCommandAsync("delete from Chapter3.Associate");
                await Task.Delay(5000);
            }
        }

        private static async Task LoadData()
        {
            using (var context = new AssociateEntities())
            {
                // 添加测试数据
                Console.WriteLine("Adding Test Data");
                Console.WriteLine("=========\n");

                var assoc1 = new Associate { Name = "Janis Roberts" };
                var assoc2 = new Associate { Name = "Kevin Hodges" };
                var assoc3 = new Associate { Name = "Bill Jordan" };

                var salary1 = new AssociateSalary
                {
                    Salary = 39500M,
                    SalaryDate = DateTime.Parse("8/4/14")
                };
                var salary2 = new AssociateSalary
                {
                    Salary = 41900M,
                    SalaryDate = DateTime.Parse("2/5/15")
                };
                var salary3 = new AssociateSalary
                {
                    Salary = 33500M,
                    SalaryDate = DateTime.Parse("10/08/13")
                };

                assoc1.AssociateSalaries.Add(salary1);
                assoc2.AssociateSalaries.Add(salary2);
                assoc3.AssociateSalaries.Add(salary3);

                context.Associates.Add(assoc1);
                context.Associates.Add(assoc2);
                context.Associates.Add(assoc3);

                //异步保存
                await context.SaveChangesAsync();
                await Task.Delay(5000);
            }
        }

        private static async Task RunForEachAsyncExample()
        {
            using (var context = new AssociateEntities())
            {
                Console.WriteLine("\n\nAsync ForEach Call");
                Console.WriteLine("=========");

                //借助 ForEachAsync 方法
                await context.Associates.Include(x => x.AssociateSalaries).ForEachAsync(x =>
                {
                    Console.WriteLine("Here are the salaries for Associate {0}:", x.Name);
                    foreach (var salary in x.AssociateSalaries)
                    {
                        Console.WriteLine("\t{0}", salary.Salary);
                    }
                });

                await Task.Delay(5000);
            }
        }

        private static async Task RunToListAsyncExample()
        {
            using (var context = new AssociateEntities())
            {
                Console.WriteLine("\n\nAsync ToList Call");
                Console.WriteLine("=========");

                //借助ToListAsyn方法
                var associates = await context.Associates.Include(x => x.AssociateSalaries).OrderBy(x => x.Name)
                    .ToListAsync();
                foreach (var associate in associates)
                {
                    Console.WriteLine("Here are the salaries for Associate {0}:", associate.Name);
                    foreach (var salaryInfo in associate.AssociateSalaries)
                    {
                        Console.WriteLine("\t{0}", salaryInfo.Salary);
                    }
                }

                await Task.Delay(5000);
            }
        }

        private static async Task RunSingleOrDefaultAsyncExample()
        {
            using (var context = new AssociateEntities())
            {
                Console.WriteLine("\n\nAsync SingleOrDefault Call");
                Console.WriteLine("=========");

                var associate = await context.Associates.Include(x => x.AssociateSalaries)
                    .OrderBy(x => x.Name)
                    .FirstOrDefaultAsync(y => y.Name == "Kevin Hodges");

                Console.WriteLine("Here are the salaries for Associate {0}:", associate.Name);
                foreach (var salaryInfo in associate.AssociateSalaries)
                {
                    Console.WriteLine("\t{0}", salaryInfo.Salary);
                }

                await Task.Delay(5000);
            }
        }





    }
}
