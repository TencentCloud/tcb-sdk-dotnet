using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public partial class Collection : Query
  {
    public Collection(Core core, string coll) : base(core, coll) { }

    public Document Doc()
    {
      return this.Doc(null);
    }

    public Document Doc(string docId)
    {
      Dictionary<string, int> projection = new Dictionary<string, int>();

      Document doc = new Document(this.Core, this.Coll, docId, projection);

      return doc;
    }

    public DbCreateResponse Add(object data)
    {
      Document doc = this.Doc();
      
      return doc.Create(data);
    }

  }
}