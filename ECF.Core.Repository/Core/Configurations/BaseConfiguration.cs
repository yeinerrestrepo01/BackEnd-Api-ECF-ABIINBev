using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECF.Core.Repository.Core.Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        protected string schema;

        protected BaseConfiguration()
        {
            this.schema = "dbo";
        }

        protected BaseConfiguration(string schema)
        {
            this.schema = schema;
        }

        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
