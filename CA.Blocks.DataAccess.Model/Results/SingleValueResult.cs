using System.Collections.Generic;

namespace CA.Blocks.DataAccess.Model.Results
{
    // This is a wrapper class to return a single value result. 
    public class SingleValueResult<T>
    {
        public T Value { get; set; }
    }


    public class ResultsSet<T1, T2 >
    {
        public IList<T1> Results1 { get; set; }

        public IList<T2> Results2 { get; set; }
    }

    public class ResultsSet<T1, T2, T3>
    {
        public IList<T1> Results1 { get; set; }

        public IList<T2> Results2 { get; set; }

        public IList<T3> Results3 { get; set; }
    }

    public class ResultsSet<T1, T2, T3, T4>
    {
        public IList<T1> Results1 { get; set; }

        public IList<T2> Results2 { get; set; }

        public IList<T3> Results3 { get; set; }

        public IList<T4> Results4 { get; set; }
    }

    public class ResultsSet<T1, T2, T3, T4, T5>
    {
        public IList<T1> Results1 { get; set; }

        public IList<T2> Results2 { get; set; }

        public IList<T3> Results3 { get; set; }

        public IList<T4> Results4 { get; set; }

        public IList<T5> Results5 { get; set; }
    }

}
