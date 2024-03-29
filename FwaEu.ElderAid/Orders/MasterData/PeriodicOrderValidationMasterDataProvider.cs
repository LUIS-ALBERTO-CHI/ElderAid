﻿using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.ElderAid.Orders.MasterData
{
    public class PeriodicOrderValidationMasterDataProvider : EntityMasterDataProvider<PeriodicOrderValidationEntity, int, PeriodicOrderValidationEntityMasterDataModel, PeriodicOrderValidationEntityRepository>
    {
        public PeriodicOrderValidationMasterDataProvider(MainSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<PeriodicOrderValidationEntity, PeriodicOrderValidationEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new PeriodicOrderValidationEntityMasterDataModel(entity.Id, entity.ArticleId, entity.PatientId, entity.Organization.Id, 
                                                        entity.Quantity, entity.DefaultQuantity, entity.OrderedOn, entity.ValidatedBy.Id, entity.ValidatedOn);
        }

        protected override Expression<Func<PeriodicOrderValidationEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Organization.Name.Contains(search);
        }
    }

    public class PeriodicOrderValidationEntityMasterDataModel
    {

        public PeriodicOrderValidationEntityMasterDataModel(int id, int articleId, int patientId, int organizationId, int quantity,
                                                                int defaultQuantity, DateTime? orderedOn, int validatedById, DateTime validatedOn)
        {
            Id = id;
            ArticleId = articleId;
            PatientId = patientId;
            OrganizationId = organizationId;
            Quantity = quantity;
            DefaultQuantity = defaultQuantity;
            OrderedOn = orderedOn;
            ValidatedById = validatedById;
            ValidatedOn = validatedOn;
        }

        public int Id { get; }
        public int ArticleId { get; }

        public int PatientId { get; }
        public int OrganizationId { get; }
        public int Quantity { get;  }
        public int DefaultQuantity { get;}

        public DateTime? OrderedOn { get;  }
        public int ValidatedById { get;  }
        public DateTime ValidatedOn { get; }
    }
}