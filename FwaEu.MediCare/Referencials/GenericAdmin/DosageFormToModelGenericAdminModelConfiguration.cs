using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.GenericAdmin;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.GenericAdmin
{
    public class DosageFormModel
    {
        [Property(IsKey = true, IsEditable = false)]
        public int? Id { get; set; }

        public string Name { get; set; }
        public DateTime? UpdatedOn { get; set; }


        public class DosageFormEntityToModelGenericAdminModelConfiguration
        : EntityToModelGenericAdminModelConfiguration<DosageFormEntity, int, DosageFormModel, MainSessionContext>
        {

            public DosageFormEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider)
            : base(serviceProvider)
            {
            }

            public override string Key => nameof(DosageFormEntity);

            public override Task<bool> IsAccessibleAsync()
            {
                return Task.FromResult(true);
            }

            protected override Task FillEntityAsync(DosageFormEntity entity, DosageFormModel model)
            {
                entity.Name = model.Name;

                return Task.CompletedTask;
            }

            protected override async Task<DosageFormEntity> GetEntityAsync(DosageFormModel model)
            {
                return await this.GetRepository().GetAsync(model.Id.Value);
            }

            protected override Expression<Func<DosageFormEntity, DosageFormModel>> GetSelectExpression()
            {
                return entity => new DosageFormModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    UpdatedOn = entity.UpdatedOn,
                };
            }

            protected override bool IsNew(DosageFormModel model)
            {
                return model.Id == null;

            }

            protected override void SetIdToModel(DosageFormModel model, DosageFormEntity entity)
            {
                model.Id = entity.Id;
            }
        }
    }
}
