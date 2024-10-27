using System.Data.Entity;

namespace MedicalLIMSApi.Infrastructure.Context
{
    public partial class TrainingContext : DbContext
    {
        public TrainingContext()
            : base(DBInfo.GetInstance().ConnectionString)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
