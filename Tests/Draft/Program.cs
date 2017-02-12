using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draft
{
    class Program
    {
        public static unsafe TTo Cast<TFrom, TTo>(TFrom origin)
        {
            TTo placeholder = default(TTo);
            TypedReference trPla = __makeref(placeholder);
            TypedReference trOr = __makeref(origin);
            *(void**)&trPla = *(void**)&trOr;
            return __refvalue(trPla, TTo);
        }

        class Animal
        {
            public string Name { get; set; }
            public virtual string Voice()
                => "!!!!";
        }

        class Dog : Animal
        {
            public override string Voice()
            {
                return "Gay";
            }
        }

        class Cat: Animal
        {
            public override string Voice()
            {
                return "Mey";
            }
        }

        static void Main(string[] args)
        {
            Dog[] dogArr = new Dog[]
            {
                new Dog(),
                new Dog(),
            };
            Animal[] animalArr = dogArr;

            //animalArr[0] = new Cat(); // EXCEPTION

            Dog[] sameDogArr = (Dog[])animalArr;
            sameDogArr[0] = new Dog();


            Animal[] dogArrToo = new Animal[]
            {
                new Dog(),
                new Dog(),
            };

            dogArrToo[0] = new Dog();

            // BUT
            dogArrToo[0] = new Cat();
            // works TOO

            var realDogArr = Cast<Animal[], Dog[]>(dogArrToo);
            realDogArr[0] = new Dog();
            ((Animal[])realDogArr)[0] = new Cat();


            List<Dog> list = new List<Dog>
            {
                new Dog(),
                new Dog(),
            };
            Dog[] qwe = list.ToArray();
            object[] zxczc = qwe;
            var t1 = Cast<List<Dog>, List<Animal>>(list);
            //t1.Add(new Cat());
            t1.Add(new Dog());
            t1.Add(new Animal());
            foreach (var item in t1)
            {
                Console.WriteLine(item.Voice());
            }

            //var t = (List<Animal>)(object)list;
        }
    }
}
