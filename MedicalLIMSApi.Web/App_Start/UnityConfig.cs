using MedicalLIMSApi.Core.Interface.AnalystQualification;
using MedicalLIMSApi.Core.Interface.ApprovalProcess;
using MedicalLIMSApi.Core.Interface.AuditTrail;
using MedicalLIMSApi.Core.Interface.CalibrationArds;
using MedicalLIMSApi.Core.Interface.Common;
using MedicalLIMSApi.Core.Interface.DataReview;
using MedicalLIMSApi.Core.Interface.Indicators;
using MedicalLIMSApi.Core.Interface.Invalidations;
using MedicalLIMSApi.Core.Interface.Login;
using MedicalLIMSApi.Core.Interface.MobilePhase;
using MedicalLIMSApi.Core.Interface.OOS;
using MedicalLIMSApi.Core.Interface.QCCalibrations;
using MedicalLIMSApi.Core.Interface.QCInventory;
using MedicalLIMSApi.Core.Interface.Report;
using MedicalLIMSApi.Core.Interface.RinsingSolutions;
using MedicalLIMSApi.Core.Interface.RolePermissions;
using MedicalLIMSApi.Core.Interface.SampleAnalysis;
using MedicalLIMSApi.Core.Interface.SampleDestruction;
using MedicalLIMSApi.Core.Interface.SamplePlan;
using MedicalLIMSApi.Core.Interface.SpecValidation;
using MedicalLIMSApi.Core.Interface.StockSolutions;
using MedicalLIMSApi.Core.Interface.UtilUploads;
using MedicalLIMSApi.Core.Interface.VolumetricSolution;
using MedicalLIMSApi.Infrastructure.Repository.AnalystQualification;
using MedicalLIMSApi.Infrastructure.Repository.ApprovalProcess;
using MedicalLIMSApi.Infrastructure.Repository.AuditTrail;
using MedicalLIMSApi.Infrastructure.Repository.CalibrationArds;
using MedicalLIMSApi.Infrastructure.Repository.Common;
using MedicalLIMSApi.Infrastructure.Repository.DataReview;
using MedicalLIMSApi.Infrastructure.Repository.Indicators;
using MedicalLIMSApi.Infrastructure.Repository.Invalidations;
using MedicalLIMSApi.Infrastructure.Repository.Login;
using MedicalLIMSApi.Infrastructure.Repository.MobilePhase;
using MedicalLIMSApi.Infrastructure.Repository.OOS;
using MedicalLIMSApi.Infrastructure.Repository.QCCalibrations;
using MedicalLIMSApi.Infrastructure.Repository.QCInventory;
using MedicalLIMSApi.Infrastructure.Repository.Report;
using MedicalLIMSApi.Infrastructure.Repository.RinsingSolutions;
using MedicalLIMSApi.Infrastructure.Repository.RolePermissions;
using MedicalLIMSApi.Infrastructure.Repository.SampleAnalysis;
using MedicalLIMSApi.Infrastructure.Repository.SampleDestruction;
using MedicalLIMSApi.Infrastructure.Repository.SamplePlan;
using MedicalLIMSApi.Infrastructure.Repository.SpecValidation;
using MedicalLIMSApi.Infrastructure.Repository.StockSolutions;
using MedicalLIMSApi.Infrastructure.Repository.UtilUploads;
using MedicalLIMSApi.Infrastructure.Repository.VolumetricSolution;
using System;

using Unity;

namespace MedicalLIMSApi.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {

            container.RegisterType<IApprovalProcess, ApprovalProcessRepository>();
            container.RegisterType<ILogin, LoginRepository>();
            container.RegisterType<IAuditTrail, AuditTrailRepository>();
            container.RegisterType<IUtilUploads, UtilUploadsRepository>();
            container.RegisterType<ICommon, CommonRepository>();

            container.RegisterType<IRolePermissions, RolePermissionRepository>();
            container.RegisterType<IMobilePhase, MobilephaseRepository>();
            container.RegisterType<IInvalidations, InvalidationsRepository>();
            container.RegisterType<IReport, ReportRepository>();
            container.RegisterType<IIndicators, IndicatorsRepository>();
            container.RegisterType<IVolumetricSolution, VolumetricSolutionRepository>();
            container.RegisterType<IAnalystQualification, AnalystQualificationRepository>();
            container.RegisterType<ISamplePlan, SamplePlanRepository>();
            container.RegisterType<ISampleAnalysis, SampleAnalysisRepository>();
            container.RegisterType<IQCInventory, QCInventoryRepository>();
            container.RegisterType<ISampleDestruction, SampleDestructionRepository>();
            container.RegisterType<ISpecValidation, SpecValidationRepository>();
            container.RegisterType<IQCCalibrations, QCCalibrationsRepository>();
            container.RegisterType<IStockSolutions, StockSolutionsRepository>();
            container.RegisterType<ICalibrationArds, CalibrationArdsRepository>();
            container.RegisterType<IRinsingSolutions, RinsingSolutionsRepository>();
            container.RegisterType<IOos, OosRepository>();
            container.RegisterType<IDataReview, DataReviewRepository>();

            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}