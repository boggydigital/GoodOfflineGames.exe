﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Delegates.Convert;
using Delegates.GetFilename;
using Delegates.Format.Uri;
using Delegates.Itemize;
using Delegates.Confirm;
using Delegates.Recycle;
using Delegates.Correct;
using Delegates.Replace;
using Delegates.EnumerateIds;
using Delegates.Download;
using Delegates.Convert.Hashes;
using Delegates.Convert.Collections;
using Delegates.Confirm.ArgsTokens;
using Delegates.GetDirectory.Root;
using Delegates.GetDirectory.ProductTypes;
using Delegates.GetPath.Json;
using Delegates.Itemize.Attributes;
using Delegates.GetValue.Uri.ProductTypes;
using Delegates.GetValue.QueryParameters.ProductTypes;

using Controllers.Storage;
using Controllers.File;
using Controllers.Directory;
using Controllers.Uri;
using Controllers.Network;
using Controllers.Language;
using Controllers.Serialization.JSON;
using Controllers.Collection;
using Controllers.Validation;
using Controllers.ValidationResult;
using Controllers.Stash.ArgsDefinitions;
using Controllers.SerializedStorage.ProtoBuf;
using Controllers.Presentation;
using Controllers.Routing;
using Controllers.Status;
using Controllers.ViewUpdates;
using Controllers.InputOutput;
using Controllers.Dependencies;
using Controllers.Records.Session;
using Controllers.Data.ProductTypes;
using Controllers.Stream;

using Interfaces.Delegates.Itemize;

using Interfaces.Models.Entities;

using GOG.Models;

using GOG.Controllers.Data.ProductTypes;

using GOG.Delegates.GetPageResults;
using GOG.Delegates.GetPageResults.ProductTypes;
using GOG.Delegates.FillGaps;
using GOG.Delegates.GetDownloadSources;
using GOG.Delegates.GetUpdateIdentity;
using GOG.Delegates.DownloadProductFile;
using GOG.Delegates.GetImageUri;
using GOG.Delegates.UpdateScreenshots;
using GOG.Delegates.GetDeserialized;
using GOG.Delegates.GetDeserialized.ProductTypes;
using GOG.Delegates.Itemize;
using GOG.Delegates.Format;
using GOG.Delegates.RequestPage;
using GOG.Delegates.Confirm;

using GOG.Controllers.Authorization;

using GOG.Activities.Help;
using GOG.Activities.Authorize;
using GOG.Activities.Update;
using GOG.Activities.Update.ProductTypes;
using GOG.Activities.UpdateDownloads;
using GOG.Activities.DownloadProductFiles;
using GOG.Activities.Cleanup;
using GOG.Activities.Validate;
using GOG.Activities.Report;

using Models.Status;
using Models.QueryParameters;
using Models.Records;

using Creators.Delegates.Convert.Requests;

namespace vangogh.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dependenciesController = new DependenciesController();
            var dependenciesControllerInitializer = new DependenciesControllerInitializer(dependenciesController);

            dependenciesControllerInitializer.Initialize();

            // var getEmptyDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetEmptyDirectoryDelegate))
            //     as GetEmptyDirectoryDelegate;

            // var getTemplatesDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetTemplatesDirectoryDelegate))
            //     as GetTemplatesDirectoryDelegate;

            // var getDataDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetDataDirectoryDelegate))
            //     as GetDataDirectoryDelegate;

            // var getRecycleBinDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetRecycleBinDirectoryDelegate))
            //     as GetRecycleBinDirectoryDelegate;

            var getProductImagesDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetProductImagesDirectoryDelegate))
                as GetProductImagesDirectoryDelegate;

            var getAccountProductImagesDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetAccountProductImagesDirectoryDelegate))
                as GetAccountProductImagesDirectoryDelegate;

            var getReportDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetReportDirectoryDelegate))
                as GetReportDirectoryDelegate;

            var getMd5DirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetMd5DirectoryDelegate))
                as GetMd5DirectoryDelegate;

            var getProductFilesRootDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetProductFilesRootDirectoryDelegate))
                as GetProductFilesRootDirectoryDelegate;

            var getScreenshotsDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetScreenshotsDirectoryDelegate))
                as GetScreenshotsDirectoryDelegate;

            var getProductFilesDirectoryDelegate = dependenciesController.GetInstance(
                typeof(GetProductFilesDirectoryDelegate))
                as GetProductFilesDirectoryDelegate;

            // var getArgsDefinitionsFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetArgsDefinitionsFilenameDelegate))
            //     as GetArgsDefinitionsFilenameDelegate;

            // var getHashesFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetHashesFilenameDelegate))
            //     as GetHashesFilenameDelegate;

            // var getAppTemplateFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetAppTemplateFilenameDelegate))
            //     as GetAppTemplateFilenameDelegate;

            // var getReportTemplateFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetReportTemplateFilenameDelegate))
            //     as GetReportTemplateFilenameDelegate;                

            // var getCookiesFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetCookiesFilenameDelegate))
            //     as GetCookiesFilenameDelegate;  

            // var getIndexFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetIndexFilenameDelegate))
            //     as GetIndexFilenameDelegate;

            // var getWishlistedFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetWishlistedFilenameDelegate))
            //     as GetWishlistedFilenameDelegate;

            // var getUpdatedFilenameDelegate = dependenciesController.GetInstance(
            //     typeof(GetUpdatedFilenameDelegate))
            //     as GetUpdatedFilenameDelegate;

            var getUriFilenameDelegate = dependenciesController.GetInstance(
                typeof(GetUriFilenameDelegate))
                as GetUriFilenameDelegate;

            var getReportFilenameDelegate = dependenciesController.GetInstance(
                typeof(GetReportFilenameDelegate))
                as GetReportFilenameDelegate;

            var getValidationFilenameDelegate = dependenciesController.GetInstance(
                typeof(GetValidationFilenameDelegate))
                as GetValidationFilenameDelegate;

            // var getArgsDefinitionsPathDelegate = dependenciesController.GetInstance(
            //     typeof(GetArgsDefinitionsPathDelegate))
            //     as GetArgsDefinitionsPathDelegate;

            // var getHashesPathDelegate = dependenciesController.GetInstance(
            //     typeof(GetHashesPathDelegate))
            //     as GetHashesPathDelegate;

            // var getAppTemplatePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetAppTemplatePathDelegate))
            //     as GetAppTemplatePathDelegate;

            // var getReportTemplatePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetReportTemplatePathDelegate))
            //     as GetReportTemplatePathDelegate;

            // var getCookiePathDelegate = dependenciesController.GetInstance(
            //     typeof(GetCookiesPathDelegate))
            //     as GetCookiesPathDelegate;

            var getGameDetailsFilesPathDelegate = dependenciesController.GetInstance(
                typeof(GetGameDetailsFilesPathDelegate))
                as GetGameDetailsFilesPathDelegate;

            var getValidationPathDelegate = dependenciesController.GetInstance(
                typeof(GetValidationPathDelegate))
                as GetValidationPathDelegate;

            var statusController = dependenciesController.GetInstance(
                typeof(StatusController))
                as StatusController;

            var streamController = dependenciesController.GetInstance(
                typeof(StreamController))
                as StreamController;

            var fileController = dependenciesController.GetInstance(
                typeof(FileController))
                as FileController;

            var directoryController = dependenciesController.GetInstance(
                typeof(DirectoryController))
                as DirectoryController;

            var storageController = dependenciesController.GetInstance(
                typeof(StorageController))
                as StorageController;

            var jsonSerializationController = dependenciesController.GetInstance(
                typeof(JSONSerializationController))
                as JSONSerializationController;

            // var convertBytesToStringDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertBytesToStringDelegate))
            //     as ConvertBytesToStringDelegate;

            var convertBytesToMd5HashDelegate = dependenciesController.GetInstance(
                typeof(ConvertBytesToMd5HashDelegate))
                as ConvertBytesToMd5HashDelegate;

            // var convertStringToBytesDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertStringToBytesDelegate))
            //     as ConvertStringToBytesDelegate;

            var convertStringToMd5HashDelegate = dependenciesController.GetInstance(
                typeof(ConvertStringToMd5HashDelegate))
                as ConvertStringToMd5HashDelegate;

            // var protoBufSerializedStorageController = dependenciesController.GetInstance(
            //     typeof(ProtoBufSerializedStorageController))
            //     as ProtoBufSerializedStorageController;

            // var jsonSerializedStorageController = dependenciesController.GetInstance(
            //     typeof(JSONSerializedStorageController))
            //     as JSONSerializedStorageController;

            var argsDefinitionStashController = dependenciesController.GetInstance(
                typeof(ArgsDefinitionsStashController))
                as ArgsDefinitionsStashController;

            // var appTemplateStashController = dependenciesController.GetInstance(
            //     typeof(AppTemplateStashController))
            //     as AppTemplateStashController;

            // var reportTemplateStashController = dependenciesController.GetInstance(
            //     typeof(ReportTemplateStashController))
            //     as ReportTemplateStashController;

            // var cookiesStashController = dependenciesController.GetInstance(
            //     typeof(CookiesStashController))
            //     as CookiesStashController;

            // var consoleController = dependenciesController.GetInstance(
            //     typeof(ConsoleController))
            //     as ConsoleController;

            // var formatTextToFitConsoleWindowDelegate = dependenciesController.GetInstance(
            //     typeof(FormatTextToFitConsoleWindowDelegate))
            //     as FormatTextToFitConsoleWindowDelegate;

            var consoleInputOutputController = dependenciesController.GetInstance(
                typeof(ConsoleInputOutputController))
                as ConsoleInputOutputController;

            // var formatBytesDelegate = dependenciesController.GetInstance(
            //     typeof(FormatBytesDelegate))
            //     as FormatBytesDelegate;

            // var formatSecondsDelegate = dependenciesController.GetInstance(
            //     typeof(FormatSecondsDelegate))
            //     as FormatSecondsDelegate;

            var collectionController = dependenciesController.GetInstance(
                typeof(CollectionController))
                as CollectionController;

            // var itemizeStatusChildrenDelegate = dependenciesController.GetInstance(
            //     typeof(ItemizeStatusChildrenDelegate))
            //     as ItemizeStatusChildrenDelegate;

            // var convertStatusTreeToEnumerableDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertStatusTreeToEnumerableDelegate))
            //     as ConvertStatusTreeToEnumerableDelegate;

            var applicationStatus = dependenciesController.GetInstance(
                typeof(Status))
                as Status;

            // var appTemplateController = dependenciesController.GetInstance(
            //     typeof(AppTemplateController))
            //     as AppTemplateController;

            // var reportTemplateController = dependenciesController.GetInstance(
            //     typeof(ReportTemplateController))
            //     as ReportTemplateController;

            // var formatRemainingTimeAtSpeedDelegate = dependenciesController.GetInstance(
            //     typeof(FormatRemainingTimeAtSpeedDelegate))
            //     as FormatRemainingTimeAtSpeedDelegate;

            // var getStatusAppViewModelDelegate = dependenciesController.GetInstance(
            //     typeof(GetStatusAppViewModelDelegate))
            //     as GetStatusAppViewModelDelegate;

            // var statusReportViewModelDelegate = dependenciesController.GetInstance(
            //     typeof(GetStatusReportViewModelDelegate))
            //     as GetStatusReportViewModelDelegate;

            var getStatusViewUpdateDelegate = dependenciesController.GetInstance(
                typeof(GetStatusViewUpdateDelegate))
                as GetStatusViewUpdateDelegate;

            // var consoleNotifyStatusViewUpdateController = dependenciesController.GetInstance(
            //     typeof(NotifyStatusViewUpdateController))
            //     as NotifyStatusViewUpdateController;

            // TODO: Implement a better way
            // add notification handler to drive console view updates
            // statusController.NotifyStatusChangedAsync += consoleNotifyStatusViewUpdateController.NotifyViewUpdateOutputOnRefreshAsync;

            // var constrainExecutionAsyncDelegate = dependenciesController.GetInstance(
            //     typeof(ConstrainExecutionAsyncDelegate))
            //     as ConstrainExecutionAsyncDelegate;

            // var itemizeAllRateConstrainedUrisDelegate = dependenciesController.GetInstance(
            //     typeof(ItemizeAllRateConstrainedUrisDelegate))
            //     as ItemizeAllRateConstrainedUrisDelegate;

            // var constrainRequestRateAsyncDelegate = dependenciesController.GetInstance(
            //     typeof(ConstrainRequestRateAsyncDelegate))
            //     as ConstrainRequestRateAsyncDelegate;;

            var uriController = dependenciesController.GetInstance(
                typeof(UriController))
                as UriController;

            // var cookiesSerializationController = dependenciesController.GetInstance(
            //     typeof(CookiesSerializationController))
            //     as CookiesSerializationController;

            // var cookiesController = dependenciesController.GetInstance(
            //     typeof(CookiesController))
            //     as CookiesController;

            var networkController = dependenciesController.GetInstance(
                typeof(NetworkController))
                as NetworkController;

            var downloadFromResponseAsyncDelegate = dependenciesController.GetInstance(
                typeof(DownloadFromResponseAsyncDelegate))
                as DownloadFromResponseAsyncDelegate;

            var downloadFromUriAsyncDelegate = dependenciesController.GetInstance(
                typeof(DownloadFromUriAsyncDelegate))
                as DownloadFromUriAsyncDelegate;

            var requestPageAsyncDelegate = dependenciesController.GetInstance(
                typeof(RequestPageAsyncDelegate))
                as RequestPageAsyncDelegate;

            var languageController = dependenciesController.GetInstance(
                typeof(LanguageController))
                as LanguageController;

            var itemizeGOGDataDelegate = dependenciesController.GetInstance(
                typeof(ItemizeGOGDataDelegate))
                as ItemizeGOGDataDelegate;

            var itemizeScreenshotsDelegate = dependenciesController.GetInstance(
                typeof(ItemizeScreenshotsDelegate))
                as ItemizeScreenshotsDelegate; ;

            var formatImagesUriDelegate = dependenciesController.GetInstance(
                typeof(FormatImagesUriDelegate))
                as FormatImagesUriDelegate;

            var formatScreenshotsUriDelegate = dependenciesController.GetInstance(
                typeof(FormatScreenshotsUriDelegate))
                as FormatScreenshotsUriDelegate;

            var recycleDelegate = dependenciesController.GetInstance(
                typeof(RecycleDelegate))
                as RecycleDelegate;

            var wishlistedDataController = dependenciesController.GetInstance(
                typeof(WishlistedDataController))
                as WishlistedDataController;

            var updatedDataController = dependenciesController.GetInstance(
                typeof(UpdatedDataController))
                as UpdatedDataController;

            var sessionRecordsController = dependenciesController.GetInstance(
                typeof(SessionRecordsController))
                as SessionRecordsController;

            // var convertProductRecordToIndexDelegate = dependenciesController.GetInstance(
            //     typeof(ConvertProductCoreToIndexDelegate<ProductRecords>))
            //     as ConvertProductCoreToIndexDelegate<ProductRecords>;

            // var getRecordsDirectoryDelegate = dependenciesController.GetInstance(
            //     typeof(GetRecordsDirectoryDelegate))
            //     as GetRecordsDirectoryDelegate;

            // Controllers.Data

            var productsDataController = dependenciesController.GetInstance(
                typeof(ProductsDataController))
                as ProductsDataController;

            var accountProductsDataController = dependenciesController.GetInstance(
                typeof(AccountProductsDataController))
                as AccountProductsDataController;

            var gameDetailsDataController = dependenciesController.GetInstance(
                typeof(GameDetailsDataController))
                as GameDetailsDataController;

            var gameProductDataDataController = dependenciesController.GetInstance(
                typeof(GameProductDataDataController))
                as GameProductDataDataController;

            var apiProductsDataController = dependenciesController.GetInstance(
                typeof(ApiProductsDataController))
                as ApiProductsDataController;

            var productScreenshotsDataController = dependenciesController.GetInstance(
                typeof(ProductScreenshotsDataController))
                as ProductScreenshotsDataController;

            var productDownloadsDataController = dependenciesController.GetInstance(
                typeof(ProductDownloadsDataController))
                as ProductDownloadsDataController;

            var productRoutesDataController = dependenciesController.GetInstance(
                typeof(ProductRoutesDataController))
                as ProductRoutesDataController;

            var validationResultsDataController = dependenciesController.GetInstance(
                typeof(ValidationResultsDataController))
                as ValidationResultsDataController;

            #region Activity Controllers

            var authorizeActivity = dependenciesController.GetInstance(
                typeof(AuthorizeActivity))
                as AuthorizeActivity;

            var updateProductsActivity = dependenciesController.GetInstance(
                typeof(UpdateProductsActivity))
                as UpdateProductsActivity;

            var updateAccountProductsActivity = dependenciesController.GetInstance(
                typeof(UpdateAccountProductsActivity))
                as UpdateAccountProductsActivity;

            var updateUpdatedActivity = dependenciesController.GetInstance(
                typeof(UpdateUpdatedActivity))
                as UpdateUpdatedActivity;

            var updateWishlistedActivity = dependenciesController.GetInstance(
                typeof(UpdateWishlistedActivity))
                as UpdateWishlistedActivity;

            #region Update.Products

            // dependencies for update controllers

            var getDeserializedGOGDataAsyncDelegate = dependenciesController.GetInstance(
                typeof(GetGOGDataDeserializedGOGDataAsyncDelegate))
                as GetGOGDataDeserializedGOGDataAsyncDelegate;

            var getDeserializedGameProductDataAsyncDelegate = new GetDeserializedGameProductDataAsyncDelegate(
                getDeserializedGOGDataAsyncDelegate);

            var getProductUpdateIdentityDelegate = new GetProductUpdateIdentityDelegate();
            var getGameProductDataUpdateIdentityDelegate = new GetGameProductDataUpdateIdentityDelegate();
            var getAccountProductUpdateIdentityDelegate = new GetAccountProductUpdateIdentityDelegate();

            var fillGameDetailsGapsDelegate = new FillGameDetailsGapsDelegate();

            // product update controllers

            var itemizeAllGameProductDataGapsAsyncDelegatepsDelegate = new ItemizeAllMasterDetailsGapsAsyncDelegate<Product, GameProductData>(
                productsDataController,
                gameProductDataDataController);

            UpdateMasterDetailProductActivity<Product, GameProductData> gameProductDataUpdateActivity = null;
            // var gameProductDataUpdateActivity = new UpdateMasterDetailProductActivity<Product, GameProductData>(
            //     Entity.GameProductData,
            //     getProductUpdateUriByContextDelegate,
            //     itemizeAllUserRequestedIdsOrDefaultAsyncDelegate,
            //     productsDataController,
            //     gameProductDataDataController,
            //     updatedIndexController,
            //     getDeserializedGameProductDataAsyncDelegate,
            //     getGameProductDataUpdateIdentityDelegate,
            //     statusController);

            var getApiProductDelegate = new GetDeserializedGOGModelAsyncDelegate<ApiProduct>(
                networkController,
                jsonSerializationController);

            var itemizeAllApiProductsGapsAsyncDelegate = new ItemizeAllMasterDetailsGapsAsyncDelegate<Product, ApiProduct>(
                productsDataController,
                apiProductsDataController);

            // var itemizeAllUserRequestedOrApiProductGapsAndUpdatedDelegate = new ItemizeAllUserRequestedIdsOrDefaultAsyncDelegate(
            //     itemizeAllUserRequestedIdsDelegate,
            //     itemizeAllApiProductsGapsAsyncDelegate,
            //     updatedIndexController);

            UpdateMasterDetailProductActivity<Product, ApiProduct> apiProductUpdateActivity = null;
            // var apiProductUpdateActivity = new UpdateMasterDetailProductActivity<Product, ApiProduct>(
            //     Entity.ApiProducts,
            //     getProductUpdateUriByContextDelegate,
            //     itemizeAllUserRequestedOrApiProductGapsAndUpdatedDelegate,
            //     productsDataController,
            //     apiProductsDataController,
            //     updatedIndexController,
            //     getApiProductDelegate,
            //     getProductUpdateIdentityDelegate,
            //     statusController);

            var getDeserializedGameDetailsDelegate = new GetDeserializedGOGModelAsyncDelegate<GameDetails>(
                networkController,
                jsonSerializationController);

            var confirmStringContainsLanguageDownloadsDelegate = new ConfirmStringMatchesAllDelegate(
                collectionController,
                Models.Separators.Separators.GameDetailsDownloadsStart,
                Models.Separators.Separators.GameDetailsDownloadsEnd);

            var replaceMultipleStringsDelegate = new ReplaceMultipleStringsDelegate();

            var itemizeDownloadLanguagesDelegate = new ItemizeDownloadLanguagesDelegate(
                languageController,
                replaceMultipleStringsDelegate);

            var itemizeGameDetailsDownloadsDelegate = new ItemizeGameDetailsDownloadsDelegate();

            var formatDownloadLanguagesDelegate = new FormatDownloadLanguageDelegate(
                replaceMultipleStringsDelegate);

            var convertOperatingSystemsDownloads2DArrayToArrayDelegate = new Convert2DArrayToArrayDelegate<OperatingSystemsDownloads>();

            var getDeserializedGameDetailsAsyncDelegate = new GetDeserializedGameDetailsAsyncDelegate(
                networkController,
                jsonSerializationController,
                languageController,
                formatDownloadLanguagesDelegate,
                confirmStringContainsLanguageDownloadsDelegate,
                itemizeDownloadLanguagesDelegate,
                itemizeGameDetailsDownloadsDelegate,
                replaceMultipleStringsDelegate,
                convertOperatingSystemsDownloads2DArrayToArrayDelegate,
                collectionController);

            var itemizeAllGameDetailsGapsAsyncDelegate = new ItemizeAllMasterDetailsGapsAsyncDelegate<AccountProduct, GameDetails>(
                accountProductsDataController,
                gameDetailsDataController);

            // var itemizeAllUserRequestedOrDefaultAsyncDelegate = new ItemizeAllUserRequestedIdsOrDefaultAsyncDelegate(
            //     itemizeAllUserRequestedIdsDelegate,
            //     itemizeAllGameDetailsGapsAsyncDelegate,
            //     updatedIndexController);

            UpdateMasterDetailProductActivity<AccountProduct, GameDetails> gameDetailsUpdateActivity = null;

            // var gameDetailsUpdateActivity = new UpdateMasterDetailProductActivity<AccountProduct, GameDetails>(
            //     Entity.GameDetails,
            //     getProductUpdateUriByContextDelegate,
            //     itemizeAllUserRequestedOrDefaultAsyncDelegate,
            //     accountProductsDataController,
            //     gameDetailsDataController,
            //     updatedIndexController,
            //     getDeserializedGameDetailsAsyncDelegate,
            //     getAccountProductUpdateIdentityDelegate,
            //     statusController,
            //     fillGameDetailsGapsDelegate);

            #endregion

            #region Update.Screenshots

            var updateScreenshotsAsyncDelegate = new UpdateScreenshotsAsyncDelegate(
                null, //getProductUpdateUriByContextDelegate,
                productScreenshotsDataController,
                networkController,
                itemizeScreenshotsDelegate,
                statusController);

            var updateScreenshotsActivity = new UpdateScreenshotsActivity(
                productsDataController,
                productScreenshotsDataController,
                updateScreenshotsAsyncDelegate,
                statusController);

            #endregion

            // dependencies for download controllers

            var getProductImageUriDelegate = new GetProductImageUriDelegate();
            var getAccountProductImageUriDelegate = new GetAccountProductImageUriDelegate();

            var getProductsImagesDownloadSourcesAsyncDelegate = new GetProductCoreImagesDownloadSourcesAsyncDelegate<Product>(
                updatedDataController,
                productsDataController,
                formatImagesUriDelegate,
                getProductImageUriDelegate,
                statusController);

            var getAccountProductsImagesDownloadSourcesAsyncDelegate = new GetProductCoreImagesDownloadSourcesAsyncDelegate<AccountProduct>(
                updatedDataController,
                accountProductsDataController,
                formatImagesUriDelegate,
                getAccountProductImageUriDelegate,
                statusController);

            var getScreenshotsDownloadSourcesAsyncDelegate = new GetScreenshotsDownloadSourcesAsyncDelegate(
                productScreenshotsDataController,
                formatScreenshotsUriDelegate,
                getScreenshotsDirectoryDelegate,
                fileController,
                statusController);

            var routingController = new RoutingController(
                productRoutesDataController,
                statusController);

            var itemizeGameDetailsManualUrlsAsyncDelegate = new ItemizeGameDetailsManualUrlsAsyncDelegate(
                gameDetailsDataController);

            var itemizeGameDetailsDirectoriesAsyncDelegate = new ItemizeGameDetailsDirectoriesAsyncDelegate(
                itemizeGameDetailsManualUrlsAsyncDelegate,
                getProductFilesDirectoryDelegate);

            var itemizeGameDetailsFilesAsyncDelegate = new ItemizeGameDetailsFilesAsyncDelegate(
                itemizeGameDetailsManualUrlsAsyncDelegate,
                routingController,
                getGameDetailsFilesPathDelegate,
                statusController);

            // product files are driven through gameDetails manual urls
            // so this sources enumerates all manual urls for all updated game details
            var getManualUrlDownloadSourcesAsyncDelegate = new GetManualUrlDownloadSourcesAsyncDelegate(
                updatedDataController,
                gameDetailsDataController,
                itemizeGameDetailsManualUrlsAsyncDelegate,
                statusController);

            // schedule download controllers

            var updateProductsImagesDownloadsActivity = new UpdateDownloadsActivity(
                Entity.ProductImages,
                getProductsImagesDownloadSourcesAsyncDelegate,
                getProductImagesDirectoryDelegate,
                fileController,
                productDownloadsDataController,
                accountProductsDataController,
                productsDataController,
                statusController);

            var updateAccountProductsImagesDownloadsActivity = new UpdateDownloadsActivity(
                Entity.AccountProductImages,
                getAccountProductsImagesDownloadSourcesAsyncDelegate,
                getAccountProductImagesDirectoryDelegate,
                fileController,
                productDownloadsDataController,
                accountProductsDataController,
                productsDataController,
                statusController);

            var updateScreenshotsDownloadsActivity = new UpdateDownloadsActivity(
                Entity.Screenshots,
                getScreenshotsDownloadSourcesAsyncDelegate,
                getScreenshotsDirectoryDelegate,
                fileController,
                productDownloadsDataController,
                accountProductsDataController,
                productsDataController,
                statusController);

            var updateProductFilesDownloadsActivity = new UpdateDownloadsActivity(
                Entity.ProductFiles,
                getManualUrlDownloadSourcesAsyncDelegate,
                getProductFilesDirectoryDelegate,
                fileController,
                productDownloadsDataController,
                accountProductsDataController,
                productsDataController,
                statusController);

            // downloads processing

            var formatUriRemoveSessionDelegate = new FormatUriRemoveSessionDelegate();

            var confirmValidationExpectedDelegate = new ConfirmValidationExpectedDelegate();

            var formatValidationUriDelegate = new FormatValidationUriDelegate(
                getValidationFilenameDelegate,
                formatUriRemoveSessionDelegate);

            var formatValidationFileDelegate = new FormatValidationFileDelegate(
                getValidationPathDelegate);

            var downloadValidationFileAsyncDelegate = new DownloadValidationFileAsyncDelegate(
                formatUriRemoveSessionDelegate,
                confirmValidationExpectedDelegate,
                formatValidationFileDelegate,
                getMd5DirectoryDelegate,
                formatValidationUriDelegate,
                fileController,
                downloadFromUriAsyncDelegate,
                statusController);

            var downloadManualUrlFileAsyncDelegate = new DownloadManualUrlFileAsyncDelegate(
                networkController,
                formatUriRemoveSessionDelegate,
                routingController,
                downloadFromResponseAsyncDelegate,
                downloadValidationFileAsyncDelegate,
                statusController);

            var downloadProductImageAsyncDelegate = new DownloadProductImageAsyncDelegate(downloadFromUriAsyncDelegate);

            var productsImagesDownloadActivity = new DownloadFilesActivity(
                Entity.ProductImages,
                productDownloadsDataController,
                downloadProductImageAsyncDelegate,
                statusController);

            var accountProductsImagesDownloadActivity = new DownloadFilesActivity(
                Entity.AccountProductImages,
                productDownloadsDataController,
                downloadProductImageAsyncDelegate,
                statusController);

            var screenshotsDownloadActivity = new DownloadFilesActivity(
                Entity.Screenshots,
                productDownloadsDataController,
                downloadProductImageAsyncDelegate,
                statusController);

            var productFilesDownloadActivity = new DownloadFilesActivity(
                Entity.ProductFiles,
                productDownloadsDataController,
                downloadManualUrlFileAsyncDelegate,
                statusController);

            // validation controllers

            var validationResultController = new ValidationResultController();

            var convertFileToMd5HashDelegate = new ConvertFileToMd5HashDelegate(
                storageController,
                convertStringToMd5HashDelegate);

            var productFileValidationController = new FileValidationController(
                confirmValidationExpectedDelegate,
                fileController,
                streamController,
                convertBytesToMd5HashDelegate,
                validationResultController,
                statusController);

            var validateProductFilesActivity = new ValidateProductFilesActivity(
                getProductFilesDirectoryDelegate,
                getUriFilenameDelegate,
                formatValidationFileDelegate,
                productFileValidationController,
                validationResultsDataController,
                gameDetailsDataController,
                itemizeGameDetailsManualUrlsAsyncDelegate,
                updatedDataController,
                routingController,
                statusController);

            #region Cleanup

            var itemizeAllGameDetailsDirectoriesAsyncDelegate = new ItemizeAllGameDetailsDirectoriesAsyncDelegate(
                gameDetailsDataController,
                itemizeGameDetailsDirectoriesAsyncDelegate,
                statusController);

            var itemizeAllProductFilesDirectoriesAsyncDelegate = new ItemizeAllProductFilesDirectoriesAsyncDelegate(
                getProductFilesRootDirectoryDelegate,
                directoryController,
                statusController);

            var itemizeDirectoryFilesDelegate = new ItemizeDirectoryFilesDelegate(directoryController);

            var directoryCleanupActivity = new CleanupActivity(
                Entity.Directories,
                itemizeAllGameDetailsDirectoriesAsyncDelegate, // expected items (directories for gameDetails)
                itemizeAllProductFilesDirectoriesAsyncDelegate, // actual items (directories in productFiles)
                itemizeDirectoryFilesDelegate, // detailed items (files in directory)
                formatValidationFileDelegate, // supplementary items (validation files)
                recycleDelegate,
                directoryController,
                statusController);

            var itemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate =
                new ItemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate(
                    updatedDataController,
                    gameDetailsDataController,
                    itemizeGameDetailsFilesAsyncDelegate,
                    statusController);

            var itemizeAllUpdatedProductFilesAsyncDelegate =
                new ItemizeAllUpdatedProductFilesAsyncDelegate(
                    updatedDataController,
                    gameDetailsDataController,
                    itemizeGameDetailsDirectoriesAsyncDelegate,
                    directoryController,
                    statusController);

            var itemizePassthroughDelegate = new ItemizePassthroughDelegate();

            var fileCleanupActivity = new CleanupActivity(
                Entity.Files,
                itemizeAllUpdatedGameDetailsManualUrlFilesAsyncDelegate, // expected items (files for updated gameDetails)
                itemizeAllUpdatedProductFilesAsyncDelegate, // actual items (updated product files)
                itemizePassthroughDelegate, // detailed items (passthrough)
                formatValidationFileDelegate, // supplementary items (validation files)
                recycleDelegate,
                directoryController,
                statusController);

            var cleanupUpdatedActivity = new CleanupUpdatedActivity(
                updatedDataController,
                statusController);

            #endregion

            #region Help

            var helpActivity = new HelpActivity(
                statusController);

            #endregion

            #region Report Task Status 

            var reportFilePresentationController = new FilePresentationController(
                getReportDirectoryDelegate,
                getReportFilenameDelegate,
                streamController);

            var fileNotifyStatusViewUpdateController = new NotifyStatusViewUpdateController(
                getStatusViewUpdateDelegate,
                reportFilePresentationController);

            var reportActivity = new ReportActivity(
                fileNotifyStatusViewUpdateController,
                statusController);

            #endregion

            #endregion

            #region ArgsDefinition and args

            var argsDefinitions = await argsDefinitionStashController.GetDataAsync(applicationStatus);

            if (args == null ||
                args.Length == 0)
                args = argsDefinitions.DefaultArgs.Split(" ");

            var convertArgsToRequestsDelegateCreator =
                new ConvertArgsToRequestsDelegateCreator(
                    argsDefinitions,
                    argsDefinitionStashController,
                    collectionController,
                    dependenciesController);

            var confirmLikelyTokenTypeDelegate = dependenciesController.GetInstance(
                typeof(ConfirmLikelyTokenTypeDelegate))
                as ConfirmLikelyTokenTypeDelegate;

            // var convertArgsToRequestsDelegate =
            //     convertArgsToRequestsDelegateCreator.CreateDelegate();

            //var requests = convertArgsToRequestsDelegate.Convert(args);                                

            #endregion

            #region Core Activities Loop

            // foreach (var activityContext in activityContextQueue)
            // {
            //     if (!activityContextToActivityControllerMap.ContainsKey(activityContext))
            //     {
            //         await statusController.WarnAsync(
            //             applicationStatus,
            //             activityContextController.ToString(activityContext) + " is not mapped to an Activity.");
            //         continue;
            //     }

            //     var activity = activityContextToActivityControllerMap[activityContext];
            //     try
            //     {
            //         await activity.ProcessActivityAsync(applicationStatus);
            //     }
            //     catch (AggregateException ex)
            //     {
            //         var itemizeInnerExceptionsDelegate = new ItemizeInnerExceptionsDelegate();
            //         var convertTreeToEnumerableDelegate = new ConvertTreeToEnumerableDelegate<Exception>(itemizeInnerExceptionsDelegate);

            //         var errorMessages = new List<string>();
            //         foreach (var innerException in convertTreeToEnumerableDelegate.Convert(ex))
            //             errorMessages.Add(innerException.Message);

            //         var combinedErrorMessages = string.Join(Models.Separators.Separators.Common.Comma, errorMessages);

            //         await statusController.FailAsync(applicationStatus, combinedErrorMessages);

            //         var failureDumpUri = "failureDump.json";
            //         await jsonSerializedStorageController.SerializePushAsync(failureDumpUri, applicationStatus, applicationStatus);

            //         await consoleInputOutputController.OutputOnRefreshAsync(
            //             "GoodOfflineGames.exe has encountered fatal error(s): " +
            //             combinedErrorMessages +
            //             $".\nPlease refer to {failureDumpUri} for further details.\n" +
            //             "Press ENTER to close the window...");

            //         consoleController.ReadLine();

            //         return;
            //     }
            // }

            #endregion

            // TODO: Present session results

            if (applicationStatus.SummaryResults != null)
            {
                foreach (var line in applicationStatus.SummaryResults)
                    await consoleInputOutputController.OutputOnRefreshAsync(string.Join(" ", applicationStatus.SummaryResults));
            }
            else
            {
                await consoleInputOutputController.OutputContinuousAsync(applicationStatus, string.Empty, "All tasks are complete. Press ENTER to exit...");
            }

            System.Console.ReadLine();
        }
    }
}