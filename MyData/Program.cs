using Eventix_Project.Data;

using(EventixContext c = new EventixContext())
{
   foreach(var item in c.EventCategories )
        Console.WriteLine(  );
}
