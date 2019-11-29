// ========================================================================= //
//                                                                           //
// Filename:  vlSDK.h                                                        //
//                                               +-----+-----+--+--+--+--+   //
// Version:   1.0                                !     !     !  !  !  !  !   //
//                                               !     !     +--+--+--+--+   //
// Author:    Fraunhofer-Institut fuer           !     !     !  !  !  !  !   //
//            Graphische Datenverarbeitung       +-----+-----+--+--+--+--+   //
//            (IGD) Abteilung A4:                !     !     !  !  !  !  !   //
//            Virtuelle und Erweiterte Realitaet !     !     +--+--+--+--+   //
//            Fraunhoferstr. 5                   !     !     !  !  !  !  !   //
//            D-64283 Darmstadt, Germany         +-----+-----+--+--+--+--+   //
//                                               ! FFFFFF hh !     GGGG  !   //
//            Rights: Copyright (c) 2004 by IGD. ! FFFFFF hh !    GGGGGG !   //
//            All rights reserved.               ! FFF    hh h    GG     !   //
//            IGD provides this product          ! FFFFF  hhhhhh  GG  GG !   //
//            without warranty of any kind       ! FFFFF  hhh!hhh GG  GG !   //
//            and shall not be liable for        ! FFF    hh ! hh GGGGGG !   //
//            any damages caused by the use      ! FFF    hh ! hh  GGGG  !   //
//            of this product.                   +-----------+-----------+   //
//                                                                           //
// ========================================================================= //
// -----------------------------------------------------------------------   //
//                                                                           //
// Creation Date :  Tue Jul 21 17:04:00 2015   Martin Buchner                //
//                                                                           //
// ========================================================================= //

/**
 * \file vlSDK.h
 * \brief This file contains all functions of the VisionLib SDK.
 *
 * Include this file for using the VisionLib in your software.
 *
 * \see http://www.visionlib.com
 */
 /**
  * \defgroup vlCSDK vlSDK-C API
  * \brief General C-Interface for the vlSDK, which works on all devices.
  */
/**@{*/

#ifndef VL_SDK_H
#define VL_SDK_H

#include "vlSDKDef.h"
#include "vlSDKVersion.h"

struct vlAbstractApplicationWrapper_s;
/*!
 * \brief Type which represents an AbstractApplicationWrapper.
 *
 * Please use the \ref aap "vlAbstractApplication_..." functions to manipulate
 * instances of it.
 */
typedef struct vlAbstractApplicationWrapper_s vlAbstractApplicationWrapper_t;

struct vlActionWrapper_s;
/*!
 * \internal
 * \brief Type which represents an ActionWrapper.
 *
 * Please use the \ref action "vlActionWrapper_..." functions to manipulate
 * instances of it.
 */
typedef struct vlActionWrapper_s vlActionWrapper_t;

struct vlActionPipeWrapper_s;
/*!
 * \internal
 * \brief Type which represents an ActionPipe.
 *
 * Please use the \ref actionPipe "vlActionPipe_..." functions to manipulate
 * instances of it.
 */
typedef struct vlActionPipeWrapper_s vlActionPipeWrapper_t;

struct vlDataBaseWrapper_s;
/*!
 * \internal
 * \brief Type which represents a DataBaseWrapper.
 *
 * Please use the \ref dataBase "vlDataBaseWrapper_..." functions to manipulate
 * instances of it.
 */
typedef struct vlDataBaseWrapper_s vlDataBaseWrapper_t;

struct vlDataSetWrapper_s;
/*!
 * \internal
 * \brief Type which represents a DataSetWrapper.
 *
 * Please use the \ref dataSet "vlDataSetWrapper_..." functions to manipulate
 * instances of it.
 */
typedef struct vlDataSetWrapper_s vlDataSetWrapper_t;

struct vlImageWrapper_s;
/*!
 * \brief Type which represents an ImageWrapper.
 *
 * Please use the \ref image "vlImageWrapper_..." functions to manipulate
 * instances of it.
 */
typedef struct vlImageWrapper_s vlImageWrapper_t;

struct vlExtrinsicDataWrapper_s;
/*!
 * \brief Type which represents an ExtrinsicDataWrapper.
 *
 * Please use the \ref extrinsic "vlExtrinsicDataWrapper_..." functions to
 * manipulate instances of it.
 */
typedef struct vlExtrinsicDataWrapper_s vlExtrinsicDataWrapper_t;

struct vlIntrinsicDataWrapper_s;
/*!
 * \brief Type which represents an IntrinsicDataWrapper.
 *
 * Please use the \ref intrinsic "vlIntrinsicDataWrapper_..." functions to manipulate
 * instances of it.
 */
typedef struct vlIntrinsicDataWrapper_s vlIntrinsicDataWrapper_t;

struct vlWorker_s;
/*!
 * \brief Type which represents a Worker.
 *
 * Please use the \ref worker "vlWorker_..." functions to manipulate instances of
 * it.
 */
typedef struct vlWorker_s vlWorker_t;

/*!
 * Log levels.
 */
typedef enum
{
    /*! Log level. */
    VL_LOG_LOG = 0,
    /*! Fatal level. */
    VL_LOG_FATAL = 1,
    /*! Warning level. */
    VL_LOG_WARNING = 2,
    /*! Notice level. */
    VL_LOG_NOTICE = 3,
    /*! Info level. */
    VL_LOG_INFO = 4,
    /*! Debug level. */
    VL_LOG_DEBUG = 5
} vlLogLevel;

/*!
 * Screen orientations.
 */
typedef enum
{
    VL_RENDER_ROTATION_CCW_0 = 0,
    VL_RENDER_ROTATION_CCW_90 = 2,
    VL_RENDER_ROTATION_CCW_180 = 1,
    VL_RENDER_ROTATION_CCW_270 = 3,
} vlRenderRotation;

/*!
 * Internal image formats.
 */
typedef enum
{
    /*! Unsupported image format. */
    VL_IMAGE_FORMAT_UNDEFINED = 0,
    /*! Grey value image. */
    VL_IMAGE_FORMAT_GREY = 1,
    /*! Image with a red, green and blue channel. */
    VL_IMAGE_FORMAT_RGB = 2,
    /*! Image with a red, green, blue and alpha channel. */
    VL_IMAGE_FORMAT_RGBA = 3,
} vlImageFormat;

/*!
 * \brief A pointer to a callback function which receives a boolean value as
 *        parameter.
 *
 * \param data Boolean parameter. The meaning depends on the context.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackBool)(
    bool data, void* clientData);

/*!
 * \brief A pointer to a callback function which receives a zero terminated
 *        string as parameter.
 *
 * \param data Zero terminated string. The meaning depends on the context.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackZString)(
    const char data[], void* clientData);

/*!
 * \brief A pointer to a callback function which receives two zero terminated
 * string which contain JSON data.
 *
 * \param error Zero terminated string with JSON data. This will be NULL, if no
 *        error occurred. The JSON format of the error object depends on the
 *        context.
 * \param data Zero terminated string with JSON data. This could be NULL, if an
 *        error occurred. The JSON format of the data object depends on the
 *        context.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackJsonString)(
    const char error[], const char data[], void* clientData);

/*!
 * \brief A pointer to a callback function which receives a zero terminated
 * description string (usually in JSON) and a binary buffer which plain binary data.
 * Please note that the use of this function might change in future and is considered as BETA!
 *
 * NOTE: The passed data pointer should be released using the vlReleaseBinaryBuffer function.
 *
 * \param description Zero terminated string with JSON data.
 *        The JSON format of the  object depends on the context.
 * \param data Binary data pointer (might be Zero). The context specific description
 *        might point to offsets within the binary data pointer.
 * \param size Size of the binary data.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackJsonAndBinaryString)(
    const char description[], const unsigned char data[], unsigned int size,
    void* clientData);

/*!
 * \brief A pointer to a callback function which receives a pointer to an
 *        \ref image "ImageWrapper" object as parameter.
 *
 * \param image Pointer to an ImageWrapper object. You can use the
 *        \ref image "vlImageWrapper_..."" functions to work with it. Please
 *        notice, that the object is only valid inside the callback and it
 *        will automatically get deleted afterwards.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackImageWrapper)(
    vlImageWrapper_t* image, void* clientData);

/*!
 * \brief A pointer to a callback function which receives a pointer to an
 *        \ref extrinsic "ExtrinsicDataWrapper" object as parameter. Please
 *        notice, that the object is only valid inside the callback and it
 *        will automatically get deleted afterwards.
 *
 * \param extrinsicData Pointer to an ExtrinsicDataWrapper object. You can use
 *        the \ref extrinsic "vlExtrinsicDataWrapper_..."" functions to work
 *        with it.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackExtrinsicDataWrapper)(
    vlExtrinsicDataWrapper_t* extrinsicData, void* clientData);

/*!
 * \brief A pointer to a callback function which receives a pointer to an
 *        \ref intrinsic "IntrinsicDataWrapper" object as parameter. Please
 *        notice, that the object is only valid inside the callback and it
 *        will automatically get deleted afterwards.
 *
 * \param intrinsicData Pointer to an IntrinsicDataWrapper object. You can use
 *        the \ref intrinsic "vlIntrinsicDataWrapper_..."" functions to work
 *        with it.
 * \param clientData Pointer value which was initially received from the user.
 *        This can be used to invoke a member function.
 */
typedef void (VL_CALLINGCONVENTION *vlCallbackIntrinsicDataWrapper)(
    vlIntrinsicDataWrapper_t* intrinsicData, void* clientData);

#ifdef __cplusplus
extern "C"
{
#endif // __cplusplus
    /*!
     * \defgroup global Global
     *
     * \brief Global functions which can be called without a corresponding
     * object which handles the request.
     */

    /*!
     * \ingroup global
     * \brief Returns the major version number of the VisionLib plugin.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlGetVersionMajor();

    /*!
     * \ingroup global
     * \brief Returns the minor version number of the VisionLib plugin.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlGetVersionMinor();

    /*!
     * \ingroup global
     * \brief Returns the revision version number of the VisionLib plugin.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlGetVersionRevision();
    /*!
     * \ingroup global
     * \brief Copies the version string of the VisionLib plugin into a buffer.
     *
     * \param version Buffer for storing the version string.
     * \param maxSize The size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlGetVersionString(
        char version[], unsigned int maxSize);

    /*!
    * \ingroup global
    * \brief Copies the version hash of the VisionLib plugin into a buffer.
    *
    * \param version Buffer for storing the version hash.
    * \param maxSize The size of the buffer.
    * \returns \c true, on success; \c false otherwise.
    */
    VL_SDK_API bool VL_CALLINGCONVENTION vlGetVersionHashString(
        char version[], unsigned int maxSize);

    /*!
    * \ingroup global
    * \brief Copies the version timestamp of the VisionLib plugin into a buffer.
    *
    * \param version Buffer for storing the version timestamp.
    * \param maxSize The size of the buffer.
    * \returns \c true, on success; \c false otherwise.
    */
    VL_SDK_API bool VL_CALLINGCONVENTION vlGetVersionTimestampString(
        char versionTimeStamp[], unsigned int maxSize);

    /*!
     * \ingroup global
     * \brief Registers a log listener.
     *
     * \param fn Listener function which will receive all VisionLib log
     *        messages.
     * \param clientData The listener will be notified with the given pointer
     *        value. This can be used to invoke a member function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlAddLogListener(
        vlCallbackZString fn, void* clientData);

    /*!
     * \ingroup global
     * \brief Unregisters a log listener.
     *
     * \param fn Listener, which should be removed.
     * \param clientData Pointer value which was used during the registration.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlRemoveLogListener(
        vlCallbackZString fn, void* clientData);

    /*!
     * \ingroup global
     * \brief Removes all log listeners.
     *
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlClearLogListeners();

    /*!
     * \ingroup global
     * \brief Enables log buffering.
     *
     * If log buffering is enabled, then log messages will not get dispatched
     * immediately. Instead they will get buffered and one must continuously
     * call the vlFlushLogBuffer function in order to dispatch the buffered log
     * messages to the registered listeners.
     *
     * This has the advantage, that registered listeners will not get notified
     * from some arbitrary thread, which would require proper thread
     * synchronization.
     *
     * By default log buffering is disabled.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlEnableLogBuffer();

    /*!
     * \ingroup global
     * \brief Disables log buffering.
     *
     * If log buffering is disabled, then log messages will get dispatched
     * immediately. This might happen from a different thread. Therefore one
     * must make sure, that registered log listeners are thread-safe.
     *
     * With disabled log buffering, calling the vlFlushLogBuffer function is
     * not necessary.
     *
     * By default log buffering is disabled.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDisableLogBuffer();

    /*!
     * \ingroup global
     * \brief Sets the maximum number of log messages in the log buffer.
     *
     * If log buffering is enabled, then log messages will get buffered. In
     * order to not allocate too much memory, the size of the log buffer is
     * limited to a certain number of entries. If there are too many log
     * messages in the buffer, then the oldest message will get removed.
     *
     * By default the maximum number of buffer entries is 32.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlSetLogBufferSize(
        unsigned int maxEntries);

    /*!
     * \ingroup global
     * \brief Notifies registered log listeners of all buffered log messages.
     *
     * If log buffering is enabled, then log messages will not get dispatched
     * immediately. Instead they will get buffered and one must continuously
     * call the vlFlushLogBuffer function in order to dispatch the buffered log
     * messages to the registered listeners.
     *
     * Failing to call vlFlushLogBuffer with enabled log buffering will have
     * the effect, that registered log listeners will not get notified of any
     * log messages and old log messages will be lost forever.
     *
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlFlushLogBuffer();

    /*!
     * \ingroup global
     * \brief Gets the current log level.
     *
     * \returns The log level (0: log, 1: fatal, 2: warning, 3: notice,
     *          4: info, 5: debug).
     */
    VL_SDK_API int VL_CALLINGCONVENTION vlGetLogLevel();

    /*!
     * \ingroup global
     * \brief Sets the log level.
     *
     * It is recommended to set the log level  to 2 (warning) during
     * development, otherwise there will be too many messages. Only for
     * debugging purposes it might be useful to increase the log level. Before
     * deploying your application you should set the log level to 0 (log) or
     * 1 (fatal).
     *
     * \param level New log level. (0: log, 1: fatal, 2: warning, 3: notice,
     *        4: info, 5: debug).
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlSetLogLevel(int level);

    /*!
     * \ingroup global
     * \brief Logs the given message as VisionLib log.
     *
     * \param message Zero terminated string with the message.
     * \param level Log level (0: log, 1: fatal, 2: warning, 3: notice,
     *         4: info, 5: debug).
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlLog(
        const char message[], int level);

    /*!
     * \ingroup global
     * \brief Not used right now.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlSetScreenOrientation(
        int screenOrientation);

    /*!
     * \defgroup aap AbstractApplicationWrapper
     *
     * \brief Functions for managing AbstractApplicationWrapper objects.
     *
     * The AbstractApplicationWrapper is a wrapper for the AbstractApplication.
     * The AbstractApplication represents the tracking context and it stores
     * the global ActionPipe and DataSet.
     */

    /*!
     * \ingroup aap
     * \brief Creates an AbstractApplicationWrapper object.
     *
     * Use ::vlDelete_AbstractApplicationWrapper after usage to avoid
     * memory leaks.
     *
     * \returns Pointer to an AbstractApplicationWrapper object.
     */
    VL_SDK_API vlAbstractApplicationWrapper_t* VL_CALLINGCONVENTION
        vlNew_AbstractApplicationWrapper();

    /*!
     * \ingroup aap
     * \brief Deletes an AbstractApplicationWrapper object.
     *
     * Call this function after you are done using your
     * AbstractApplicationWrapper object to avoid memory leaks.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_AbstractApplicationWrapper(
        vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Returns whether any features are used, which are not licensed.
     *
     * The 'blocked features' status will be re-evaluated from time to time.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, if unlicensed features are used; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_FoundBlockedFeatures(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Simulates the usage of blocked features.
     *
     * This can be used for testing unlicensed behaviour on machines with a
     * valid license.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_ActivateFoundBlockedFeatures(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Copies the host ID of the current machine into the provided
     *        buffer as zero terminated string.
     *
     * The host ID is necessary for generating a license file.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param hostID Buffer for storing the host ID as zero terminated string.
     *        The string will be empty if no host ID is available on the system.
     * \param maxSize Size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetHostID(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            char hostID[], unsigned int maxSize);

    /*!
     * \ingroup aap
     * \brief Copies the bundle ID of the current application into the provided
     *        buffer as zero terminated string.
     *
     * The bundle ID is necessary for generating a license file.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param bundleID Buffer for storing the bundle ID as zero terminated
     *        string. The string will be empty if no bundle ID is available on
     *        the system.
     * \param maxSize Size of the buffer.
     * \returns \c true, on success; \c false otherwise or not available.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetBundleID(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            char bundleID[], unsigned int maxSize);
    /*!
     * \ingroup aap
     * \brief Sets the path of the license file.
     *
     * This is mandatory for being able to run the tracking. Alternatively you
     * can inject the license file from memory using the
     * vlAbstractApplicationWrapper_SetLicenseFileData function.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param licenseFilePath Zero terminated string with a path to the license
     *        file.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_SetLicenseFilePath(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char *licenseFilePath);

    /*!
     * \ingroup aap
     * \brief Allows to inject the license data from memory.
     *
     * This is mandatory for being able to run the tracking. Alternatively you
     * can load the license data from a file using the
     * vlAbstractApplicationWrapper_SetLicenseFilePath function. If the
     * vlAbstractApplicationWrapper_SetLicenseFileData was previously used and
     * now the license data should be read from a file, then please set the
     * license file data to an empty string first.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param licenseFileData Zero terminated string with the license data.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_SetLicenseFileData(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char *licenseFileData);

    /*!
     * \ingroup aap
     * \brief Retrieves actual license information as a JSON encoded string.
     *
     * The string is JSON encoded and can be used for examining available
     * license features. It can only be valid after a license file has been set.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param licenseInfo Buffer for storing the license information as zero
     *        terminated JSON string. The string will be empty if no license
     *        information is available on the system.
     * \param maxSize Size of the buffer.
     * \returns \c true, on success; \c false otherwise or not available.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetLicenseInformation(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            char licenseInfo[], unsigned int maxSize);

    /*!
     * \ingroup aap
     * \brief Loads all VisionLib plugins from a specific directory.
     *
     * Many VisionLib features are implemented as plugins, which need to be
     * loaded first.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param pluginPath Plugin directory as zero terminated string. Can be an
     *        empty string in which case the plugins will  be loaded from the
     *        directory stored inside the PM_PLUGIN_PATH environment variable.
     *        If this environment variable is not defined, then the plugins
     *        will be loaded from the directory of the current executable.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_AutoLoadPlugins(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char pluginPath[]);

    /*!
     * \ingroup aap
     * \brief Sets the resource path to the given given path.
     *
     * The resource directory contains tracking pipelines and other support
     * files.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param path Path to the resource directory.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_SetResourcePath(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char path[]);

    /*!
     * \ingroup aap
     * \brief Returns the type of the loaded tracking pipeline.
     *
     * This only works for tracking configurations loaded from a vl-file or
     * vl-string.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param trackerType Buffer for storing the tracker type as zero
     *        terminated string.
     * \param maxSize Size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetTrackerType(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            char trackerType[], unsigned int maxSize);

    /*!
     * \ingroup aap
     * \brief Clears the ActionPipe and DataSet.
     *
     * Please notice, that all wrapper objects for DataBase objects and
     * Actions will be left in an invalid state after calling this function.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_ClearProject(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Loads the specified tracking configuration XML file.
     *
     * Please notice, that all wrapper objects for DataBase objects and
     * Actions will be left in an invalid state after calling this function.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param filename Filename of the tracking configuration.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_LoadProjectData(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char filename[]);

    /*!
     * \ingroup aap
     * \brief Loads the given string as tracking configuration.
     *
     * Please notice, that all wrapper objects for DataBase objects and
     * Actions will be left in an invalid state after calling this function.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param str String with the tracking configuration.
     * \param fakeFilename Filename which will be used to determine the type
     *        of the tracking configuration (vl / PM) and for resolving
     *        relative file paths and the type of the tracking configuration.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_LoadProjectDataFromString(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char str[], const char fakeFilename[]);

    /*!
     * \internal
     * \ingroup aap
     * \brief Returns the main ActionPipe of the AbstractApplication.
     *
     * You must call ::vlDelete_ActionPipeWrapper function after you are done
     * using the returned object. Failure to do so will result in leaked
     * memory.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns Pointer to an ActionPipeWrapper object on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlActionPipeWrapper_t* VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetActionPipe(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \internal
     * \ingroup aap
     * \brief Returns the main DataSet of the AbstractApplication.
     *
     * You must call ::vlDelete_DataBaseWrapper function after you are done
     * using the returned object. Failure to do so will result in leaked
     * memory.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns Pointer to an DataBaseWrapper object on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlDataSetWrapper_t* VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetDataSet(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \internal
     * \ingroup aap
     * \brief Initializes all Actions.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, if the Actions were initialized successfully;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_InitActions(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \internal
     * \ingroup aap
     * \brief Sequentially executes all Actions.
     *
     * The execution order is depth first and the Actions will run one by one.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, if the Actions were executed successfully;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_ApplyActions(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Registers the current thread for the AbstractApplication.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_RegisterThread(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \ingroup aap
     * \brief Copies a JSON string with information about the current device
     *        into the provided buffer.
     *
     * It also contains information about the cameras currently connected to
     * the system.
     *
     * A typical device info might look like this:
     * \code{.json}
     * {
     *     "appID": "com.unity3d.UnityEditor5.x",
     *     "availableCameras": [{
     *         "cameraName": "HD Webcam C525",
     *         "deviceID": "HDWebcamC525",
     *         "internalID": "0x400000046d0826",
     *         "position": "unknown",
     *         "prefRes": "640x480"
     *     }],
     *     "cameraAllowed": false,
     *     "currentDisplayOrientation": 0,
     *     "hasWebServer": false,
     *     "internalModelID": "x86_64",
     *     "manufacture": "Apple",
     *     "model": "Mac",
     *     "modelVersion": "Mac",
     *     "nativeResX": 242,
     *     "nativeResY": 0,
     *     "numberOfProcessors": 1,
     *     "os": "macOS",
     *     "unifiedID": "1467016391MY190",
     *     "usingEventLogger": false,
     *     "webServerURL": ""
     * }
     * \endcode
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param buffer Buffer for storing the JSON string.
     * \param maxSize The size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_GetDeviceInfo(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            char buffer[], unsigned int maxSize);

    /*!
     * \ingroup aap
     * \brief Adds a given URI pointing to a camera calibration database JSON
     *        file to the VisionLib.
     *
     * The VisionLib loads the added camera calibration database file before
     * loading a new tracking configuration file.
     *
     * This allows to provide a custom camera calibrations for devices for
     * which the VisionLib doesn't provide a default calibration. If a default
     * calibration with the same name already exists, then the default
     * calibration will get overwritten with the custom calibration.
     *
     * Please also have a look at [this reference](\ref cameraCalibration) for
     * more information about the camera calibration database format.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \param uri Zero terminated string with an URI pointing to the camera
     *        calibration database file.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_AddCameraCalibrationDB(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper,
            const char uri[]);

    /*!
     * \ingroup aap
     * \brief Removes all references to all manually set calibration data bases.
     *
     * The VisionLib loads the added camera calibration database file before
     * loading a new tracking configuration file. This command removes the
     * queue of file to be loaded.
     *
     * This allows to provide a custom camera calibrations for devices for
     * which the VisionLib doesn't provide a default calibration. If a default
     * calibration with the same name already exists, then the default
     * calibration will get overwritten with the custom calibration.
     *
     * Please also have a look at [this reference](\ref cameraCalibration) for
     * more information about the camera calibration database format.
     *
     * \param abstractApplicationWrapper Pointer to an
     *        AbstractApplicationWrapper object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlAbstractApplicationWrapper_ResetCameraCalibrationDB(
            vlAbstractApplicationWrapper_t* abstractApplicationWrapper);

    /*!
     * \internal
     * \defgroup action ActionWrapper
     *
     * \brief Functions for managing ActionWrapper objects.
     *
     * The ActionWrapper is a wrapper for an Action. All processing steps in
     * the VisionLib are encapsulated inside Action classes.
     */

    /*!
     * \ingroup action
     * \brief Deletes an ActionWrapper object.
     *
     * Call this function if you used the ::vlActionPipeWrapper_FindAction
     * function to acquire the object and you are now done using it.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_ActionWrapper(
        vlActionWrapper_t* actionWrapper);

    /*!
     * \ingroup action
     * \brief Returns the enabled state of the Action.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \returns \c true, if the Action is enabled; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_IsEnabled(
        vlActionWrapper_t* actionWrapper);

    /*!
     * \ingroup action
     * \brief Sets the enabled state of the Action.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \param enabled Enabled state.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_SetEnabled(
        vlActionWrapper_t* actionWrapper, bool enabled);

    /*!
     * \ingroup action
     * \brief Returns the value of an attribute as zero terminated string.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Buffer for storing the attribute value.
     * \param maxSize Maximum size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_GetAttribute(
        vlActionWrapper_t* actionWrapper,
        const char attributeName[], char value[], unsigned int maxSize);

    /*!
     * \ingroup action
     * \brief Sets the value of an attribute as zero terminated string.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Zero terminated string with the attribute value.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_SetAttribute(
        vlActionWrapper_t* actionWrapper,
        const char attributeName[], const char value[]);

    /*!
     * \internal
     * \ingroup action
     * \brief Initializes the Action.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \param dataSetWrapper Pointer to an DataSetWrapper object which will be
     *        used for initializing the Action. You can use the
     *        ::vlAbstractApplicationWrapper_GetDataSet function to retrieve
     *        the default DataSet.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_Init(
        vlActionWrapper_t* actionWrapper,
        vlDataSetWrapper_t* dataSetWrapper);

    /*!
     * \internal
     * \ingroup action
     * \brief Executes the Action.
     *
     * \param actionWrapper Pointer to an ActionWrapper object.
     * \param dataSetWrapper Pointer to an DataSetWrapper object which will be
     *        used for executing the Action. You can use the
     *        ::vlAbstractApplicationWrapper_GetDataSet function to retrieve
     *        the default DataSet.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionWrapper_Apply(
        vlActionWrapper_t* actionWrapper,
        vlDataSetWrapper_t* dataSetWrapper);

    /*!
     * \internal
     * \defgroup actionPipe ActionPipeWrapper
     *
     * \brief Functions for managing ActionPipeWrapper objects.
     *
     * The ActionPipeWrapper is a wrapper for an ActionPipe. ActionPipes
     * store collections of Actions.
     *
     * Since the ActionPipe inherits from the Action class it's also possible
     * that an ActionPipe contains other ActionsPipes (composite pattern).
     *
     * The AbstractApplication stores a global ActionPipe and executes all
     * Actions within it when calling
     * ::vlAbstractApplicationWrapper_ApplyActions. ActionsPipes usually
     * forward this apply call to their sub-actions.
     */

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Deletes an ActionPipeWrapper object.
     *
     * Call this function if you used the ::vlActionPipeWrapper_FindActionPipe
     * or the ::vlAbstractApplicationWrapper_GetActionPipe function to acquire
     * the object and you are now done using it.
     *
     * \param actionPipeWrapper Pointer to an ActionWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_ActionPipeWrapper(
        vlActionPipeWrapper_t* actionPipeWrapper);
    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Returns the enabled state of the ActionPipe.
     *
     * \param actionPipeWrapper Pointer to an ActionWrapper object.
     * \returns \c true, if the ActionPipe is enabled; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_IsEnabled(
        vlActionPipeWrapper_t* actionPipeWrapper);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Sets the enabled state of the ActionPipe.
     *
     * \param actionPipeWrapper Pointer to an ActionWrapper object.
     * \param enabled Enabled state.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_SetEnabled(
        vlActionPipeWrapper_t* actionPipeWrapper, bool enabled);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Returns the value of an attribute as zero terminated string.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Buffer for storing the attribute value.
     * \param maxSize Maximum size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_GetAttribute(
        vlActionPipeWrapper_t* actionPipeWrapper,
        const char attributeName[], char value[],
        unsigned int maxSize);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Sets the value of an attribute as zero terminated string.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Zero terminated string with the attribute value.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_SetAttribute(
        vlActionPipeWrapper_t* actionPipeWrapper,
        const char attributeName[], const char value[]);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Finds the Action with the given key.
     *
     * The Action will be searched recursively depth-first within all
     * sub-action and -actionpipes.
     *
     * You must call ::vlDelete_ActionWrapper function with the returned object
     * after you are done using it. Failure to do so will result in leaked
     * memory.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param key Zero terminated string with the key of the Action to be
     *        found.
     * \returns A pointer to an ActionWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlActionWrapper_t* VL_CALLINGCONVENTION
        vlActionPipeWrapper_FindAction(
            vlActionPipeWrapper_t* actionPipeWrapper, const char key[]);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Finds the ActionPipe with the given key.
     *
     * The ActionPipe will be searched recursively depth-first within all
     * sub-action and -actionpipes.
     *
     * You must call ::vlDelete_ActionPipeWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param key Zero terminated string with the key of the Action to be
     *        found.
     * \returns A pointer to an ActionPipeWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlActionPipeWrapper_t* VL_CALLINGCONVENTION
        vlActionPipeWrapper_FindActionPipe(
            vlActionPipeWrapper_t* actionPipeWrapper, const char key[]);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Initializes the ActionPipe.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param dataSetWrapper Pointer to an DataSetWrapper object which will be
     *        used for initializing the ActionPipe. You can use the
     *        ::vlAbstractApplicationWrapper_GetDataSet function to retrieve
     *        the default DataSet.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_Init(
        vlActionPipeWrapper_t* actionPipeWrapper,
        vlDataSetWrapper_t* dataSetWrapper);

    /*!
     * \internal
     * \ingroup actionPipe
     * \brief Executes the ActionPipe.
     *
     * \param actionPipeWrapper Pointer to an ActionPipeWrapper object.
     * \param dataSetWrapper Pointer to an DataSetWrapper object which will be
     *        used for executing the ActionPipe. You can use the
     *        ::vlAbstractApplicationWrapper_GetDataSet function to retrieve
     *        the default DataSet.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlActionPipeWrapper_Apply(
        vlActionPipeWrapper_t* actionPipeWrapper,
        vlDataSetWrapper_t* dataSetWrapper);


    /*!
     * \internal
     * \defgroup dataBase DataBaseWrapper
     *
     * \brief Functions for managing DataBaseWrapper objects.
     *
     * The DataBaseWrapper is a wrapper for a DataBase object. DataBase objects
     * represent the data which can be exchanged between Actions.
     */

    /*!
     * \ingroup dataBase
     * \brief Deletes an DataBaseWrapper object.
     *
     * Call this function if you used the ::vlDataSetWrapper_GetDataBase
     * function to acquire the object and you are now done using it.
     *
     * \param dataBaseWrapper Pointer to an DataBaseWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_DataBaseWrapper(
        vlDataBaseWrapper_t* dataBaseWrapper);

    /*!
     * \internal
     * \ingroup dataBase
     * \brief Returns the value of an attribute as zero terminated string.
     *
     * \param dataBaseWrapper Pointer to an DataBaseWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Buffer for storing the attribute value.
     * \param maxSize Maximum size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlDataBaseWrapper_GetAttribute(
        vlDataBaseWrapper_t* dataBaseWrapper,
        const char attributeName[], char value[],
        unsigned int maxSize);

    /*!
     * \internal
     * \ingroup dataBase
     * \brief Sets the value of an attribute as zero terminated string.
     *
     * \param dataBaseWrapper Pointer to an DataBaseWrapper object.
     * \param attributeName Zero terminated string the name of the attribute.
     * \param value Zero terminated string with the attribute value.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlDataBaseWrapper_SetAttribute(
        vlDataBaseWrapper_t* dataBaseWrapper,
        const char attributeName[], const char value[]);

    /*!
     * \internal
     * \defgroup dataSet DataSetWrapper
     *
     * \brief Functions for managing DataSetWrapper objects.
     *
     * The DataSetWrapper is a wrapper for the DataSet. The DataSet stores a
     * collection of DataBase objects.
     *
     * Since the DataSet inherits from the DataBase class it's possible that a
     * DataSet contains other DataSets (composite pattern).
     *
     * The AbstractApplication stores a global DataSet and forwards it to all
     * Actions when applying the Actions. That way the Actions can exchange
     * data without needing to know each other.
     */

    /*!
     * \internal
     * \ingroup dataSet
     * \brief Deletes an DataSetWrapper object.
     *
     * Call this function if you used the ::vlActionPipeWrapper_FindActionPipe
     * or the ::vlAbstractApplicationWrapper_GetActionPipe function to acquire
     * the object and you are now done using it.
     *
     * \param dataSetWrapper Pointer to an DataSetWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_DataSetWrapper(
        vlDataSetWrapper_t* dataSetWrapper);

    /*!
     * \internal
     * \ingroup dataSet
     * \brief Find the DataBase object with the given key.
     *
     * The DataBase object will be searched only within the own sub-database
     * objects. If you want to get a DataBase object within an other DataSet,
     * you also need to specify the key of the containing DataSet like this:
     * 'subDataSetKey.dataBaseKey'.
     *
     * You must call ::vlDelete_DataBaseWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param dataSetWrapper Pointer to an DataBaseWrapper object.
     * \param key Zero terminated string with the key of the DataBase object to
     *        be found.
     * \returns A pointer to an DataBaseWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlDataBaseWrapper_t* VL_CALLINGCONVENTION
        vlDataSetWrapper_GetDataBase(
            vlDataSetWrapper_t* dataSetWrapper, const char key[]);

    /*!
     * \internal
     * \ingroup dataSet
     * \brief Find the DataSet object with the given key.
     *
     * The DataSet object will be searched only within the own sub-database
     * objects. If you want to get a DataSet object within an other DataSet,
     * you also need to specify the key of the containing DataSet like this:
     * 'subDataSetKey.dataSetKey'.
     *
     * You must call ::vlDelete_DataSetWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param dataSetWrapper Pointer to an DataBaseWrapper object.
     * \param key Zero terminated string with the key of the DataSet object to
     *        be found.
     * \returns A pointer to an DataSetWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlDataSetWrapper_t* VL_CALLINGCONVENTION
        vlDataSetWrapper_GetDataSet(
            vlDataSetWrapper_t* dataSetWrapper, const char key[]);

    /*!
     * \ingroup dataSet
     * \brief Find the Image with the given key.
     *
     * The Image will be searched only within the own sub-database objects. If
     * you want to get a Image within an other DataSet, you also need to
     * specify the key of the containing DataSet like this:
     * 'subDataSetKey.imageKey'.
     *
     * You must call ::vlDelete_ImageWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param dataSetWrapper Pointer to an DataBaseWrapper object.
     * \param key Zero terminated string with the key of the Image to be found.
     * \returns A pointer to an ImageWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlImageWrapper_t* VL_CALLINGCONVENTION
        vlDataSetWrapper_GetImage(
            vlDataSetWrapper_t* dataSetWrapper, const char key[]);

    /*!
     * \ingroup dataSet
     * \brief Find the ExtrinsicData object with the given key.
     *
     * The ExtrinsicData object will be searched only within the own
     * sub-database objects. If you want to get a ExtrinsicData object within
     * an other DataSet, you also need to specify the key of the containing
     * DataSet like this: 'subDataSetKey.extrinsicDataKey'.
     *
     * You must call ::vlDelete_ExtrinsicDataWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param dataSetWrapper Pointer to an DataBaseWrapper object.
     * \param key Zero terminated string with the key of the
     *        ExtrinsicDataWrapper object to be found.
     * \returns A pointer to an ExtrinsicDataWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlExtrinsicDataWrapper_t* VL_CALLINGCONVENTION
        vlDataSetWrapper_GetExtrinsicData(
            vlDataSetWrapper_t* dataSetWrapper, const char key[]);

    /*!
     * \ingroup dataSet
     * \brief Find the IntrinsicData object with the given key.
     *
     * The IntrinsicData object will be searched only within the own
     * sub-database objects. If you want to get a IntrinsicData object within
     * an other DataSet, you also need to specify the key of the containing
     * DataSet like this: 'subDataSetKey.intrinsicDataKey'.
     *
     * You must call ::vlDelete_IntrinsicDataWrapper function with the returned
     * object after you are done using it. Failure to do so will result in
     * leaked memory.
     *
     * \param dataSetWrapper Pointer to an DataBaseWrapper object.
     * \param key Zero terminated string with the key of the
     *        IntrinsicDataWrapper object to be found.
     * \returns A pointer to an IntrinsicDataWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlIntrinsicDataWrapper_t* VL_CALLINGCONVENTION
        vlDataSetWrapper_GetIntrinsicData(
            vlDataSetWrapper_t* dataSetWrapper, const char key[]);

    /*!
     * \defgroup image ImageWrapper
     *
     * \brief Functions for managing ImageWrapper objects.
     *
     * The ImageWrapper is a wrapper for an Image object.
     */

    /*!
     * \ingroup image
     * \brief Deletes an ImageWrapper object.
     *
     * Call this function if you used the ::vlDataSetWrapper_GetImage function
     * to acquire the object and you are now done using it.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_ImageWrapper(
        vlImageWrapper_t* imageWrapper);

    /*!
     * \ingroup image
     * \brief Returns the internal type of the image.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \returns Internal type of the image. The value can be cast into a
     *          ::vlImageFormat enumeration.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlImageWrapper_GetFormat(
        vlImageWrapper_t* imageWrapper);

    /*!
     * \ingroup image
     * \brief Returns the number of bytes per pixel.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \returns The number of bytes per pixel.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION
        vlImageWrapper_GetBytesPerPixel(
            vlImageWrapper_t* imageWrapper);

    /*!
     * \ingroup image
     * \brief Returns the width of the image.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \returns The width in pixels.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlImageWrapper_GetWidth(
        vlImageWrapper_t* imageWrapper);

    /*!
     * \ingroup image
     * \brief Returns the height of the image.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \returns The height in pixels.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlImageWrapper_GetHeight(
        vlImageWrapper_t* imageWrapper);


    /*!
     * \ingroup image
     * \brief Copies the VisionLib image into the given buffer.
     *
     * Please make sure, that the buffer is large enough for storing the whole
     * image date (width * height * bytesPerPixel). The number of bytes per
     * pixel an be acquired using the ::vlImageWrapper_GetBytesPerPixel
     * function.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \param buffer Buffer with width * height * bytesPerPixel bytes of
     *        memory.
     * \param bufferSize Size of the buffer.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlImageWrapper_CopyToBuffer(
        vlImageWrapper_t* imageWrapper,
        unsigned char buffer[], unsigned int bufferSize);

    /*!
     * \ingroup image
     * \brief Copies the given buffer into the VisionLib image
     *
     * The VisionLib image will be resized according to the width and height
     * parameter.
     *
     * Please make sure, that the data stored in the buffer has the same format
     * as the image. The image format can be acquired using the
     * ::vlImageWrapper_GetFormat function.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \param buffer Buffer with the raw image data.
     * \param width New width of the image.
     * \param height New height of the image.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlImageWrapper_CopyFromBuffer(
        vlImageWrapper_t* imageWrapper, const unsigned char buffer[],
        unsigned int width, unsigned int height);

    /*!
     * \ingroup image
     * \brief Copies the given an formated buffer into the VisionLib image
     *
     * The VisionLib image will be resized according to the width and height and format
     * parameter.
     *
     * The image will be converted into internally into a RGBA format.
     *
     * \param imageWrapper Pointer to an ImageWrapper object.
     * \param buffer Buffer with the raw image data.
     * \param width New width of the image.
     * \param height New height of the image.
     * \param imageFormat The format of the image.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlImageWrapper_CopyFromBufferWithFormat(
        vlImageWrapper_t* imageWrapper, const unsigned char buffer[],
        unsigned int width, unsigned int height,vlImageFormat imageFormat);
    
    /*!
     * \defgroup extrinsic ExtrinsicDataWrapper
     *
     * \brief Functions for managing ExtrinsicDataWrapper objects.
     *
     * The ExtrinsicDataWrapper is a wrapper for an ExtrinsicData object.
     * ExtrinsicData objects represent the extrinsic camera parameters
     * (position and orientation).
     */

    /*!
     * \ingroup extrinsic
     * \brief Deletes an ExtrinsicDataWrapper object.
     *
     * Call this function if you used the vlDataSetWrapper_GetExtrinsicData
     * function to acquire the object and you are now done using it.
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_ExtrinsicDataWrapper(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper);

    /*!
     * \ingroup extrinsic
     * \brief Returns whether the current tracking pose is valid (the tracking
     *        was successful).
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \returns \c true, if the current tracking pose is valid;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_GetValid(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper);

    /*!
     * \ingroup extrinsic
     * \brief Returns the current camera pose as model-view matrix.
     *
     *  The returned matrix assumes a right-handed coordinate system and is
     *  stored in the following order (column-major order):
     *  \f[
     *   \begin{bmatrix}
     *    0 & 4 &  8 & 12\\
     *    1 & 5 &  9 & 13\\
     *    2 & 6 & 10 & 14\\
     *    3 & 7 & 11 & 15\\
     *   \end{bmatrix}
     *  \f]
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param matrix Float array with 16 elements for storing the
     *        model-view matrix.
     * \param matrixElementCount Number of elements in the given array.
     *        This should be 16.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlExtrinsicDataWrapper_GetModelViewMatrix(
            vlExtrinsicDataWrapper_t* extrinsicDataWrapper, float matrix[],
            unsigned int matrixElementCount);

    /*!
     * \ingroup extrinsic
     * \brief Returns the translation \f$t\f$ from the world coordinate system
     *        to the camera coordinate system.
     *
     * Please notice, that \f$(R,t)\f$ represents the transformation of a 3D
     * point \f$P_w\f$ from world coordinates into a 3D point \f$P_c\f$ in
     * camera coordinates: \f$P_c = RP_w + t\f$.
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param t Float array with 3 elements \f$(x,y,z)\f$ for storing the
     *        translation.
     * \param elementCount Number of elements in the given array.
     *        This should be 3.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_GetT(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, float t[],
        unsigned int elementCount);

    /*!
     * \ingroup extrinsic
     * \brief Sets the translation \f$t\f$ from the world coordinate system to
     *        the camera coordinate system.
     *
     * Please notice, that \f$(R,t)\f$ represents the transformation of a 3D
     * point \f$P_w\f$ from world coordinates into a 3D point \f$P_c\f$ in
     * camera coordinates: \f$P_c = RP_w + t\f$.
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param t Float array with 3 elements \f$(x,y,z)\f$, which contain the
     *        translation.
     * \param elementCount Number of elements in the given array.
     *        This should be 3.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_SetT(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, const float t[],
        unsigned int elementCount);

    /*!
     * \ingroup extrinsic
     * \brief Returns the rotation \f$R\f$ from the world coordinate system to
     *        the camera coordinate system.
     *
     * Please notice, that \f$(R,t)\f$ represents the transformation of a 3D
     * point \f$P_w\f$ from world coordinates into a 3D point \f$P_c\f$ in
     * camera coordinates: \f$P_c = RP_w + t\f$.
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param q Float array with 4 elements \f$(x,y,z,w)\f$ for storing the
     *        rotation as quaternion.
     * \param elementCount Number of elements in the given array.
     *        This should be 4.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_GetR(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, float q[],
        unsigned int elementCount);

    /*!
     * \ingroup extrinsic
     * \brief Sets the rotation \f$R\f$ from the world coordinate system to the
     *        camera coordinate system.
     *
     * Please notice, that \f$(R,t)\f$ represents the transformation of a 3D
     * point \f$P_w\f$ from world coordinates into a 3D point \f$P_c\f$ in
     * camera coordinates: \f$P_c = RP_w + t\f$.
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param q Float array with 4 elements \f$(x,y,z,w)\f$, which contains the
     *          rotation as quaternion.
     * \param elementCount Number of elements in the given array.
     *        This should be 4.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_SetR(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, const float q[],
        unsigned int elementCount);

    /*!
     * \ingroup extrinsic
     * \brief Returns the position \f$P_{cam}\f$ of the camera in world
     *        coordinates.
     *
     * Internally the position \f$P_{cam}\f$ will be computed from the
     * transformation \f$(R,t)\f$ which transforms a 3D point from world
     * coordinates into camera coordinates (\f$P_{cam} = -R^{-1}t\f$).
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param t Float array with 3 elements \f$(x,y,z)\f$ for storing the
     *        position.
     * \param elementCount Number of elements in the given array.
     *        This should be 3.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_GetCamPosWorld(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, float t[],
        unsigned int elementCount);

    /*!
     * \ingroup extrinsic
     * \brief Sets the position \f$P_{cam}\f$ of the camera in world
     *        coordinates.
     *
     * Internally this will be stored as a transformation \f$(R,t)\f$ of a 3D
     * point from world coordinates into camera coordinates
     * (\f$t = -RP_{cam}\f$).
     *
     * \param extrinsicDataWrapper Pointer to an ExtrinsicDataWrapper object.
     * \param t Float array with 3 elements \f$(x,y,z)\f$, which contains the
     *        position.
     * \param elementCount Number of elements in the given array.
     *        This should be 3.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlExtrinsicDataWrapper_SetCamPosWorld(
        vlExtrinsicDataWrapper_t* extrinsicDataWrapper, const float t[],
        unsigned int elementCount);

    /*!
     * \defgroup intrinsic IntrinsicDataWrapper
     *
     * \brief Functions for managing IntrinsicDataWrapper objects.
     *
     * The IntrinsicDataWrapper is a wrapper for an IntrinsicData object.
     * IntrinsicData objects represent the intrinsic camera parameters
     * (focal length, principal point, skew and distortion parameters).
     */

    /*!
     * \ingroup intrinsic
     * \brief Deletes an IntrinsicDataWrapper object.
     *
     * Call this function if you used the vlDataSetWrapper_GetIntrinsicData
     * function to acquire the object and you are now done using it.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_IntrinsicDataWrapper(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the width of the intrinsic camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns The width in pixels.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetWidth(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the height of the intrinsic camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns The height in pixels.
     */
    VL_SDK_API unsigned int VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetHeight(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the normalized focal length of the intrinsic camera
     *        calibration in x direction.
     *
     * The focal length in x direction was normalized through a division by
     * the width of the camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns Normalized focal length in x direction.
     */
    VL_SDK_API double VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetFxNorm(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the normalized focal length of the intrinsic camera
     *        calibration in y direction.
     *
     * The focal length in y direction was normalized through a division by
     * the height of the camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns Normalized focal length in y direction.
     */
    VL_SDK_API double VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetFyNorm(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the normalized skew of the intrinsic camera calibration.
     *
     * The skew was normalized through a division by the width of the camera
     * calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns Normalized skew.
     */
    VL_SDK_API double VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetSkewNorm(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the normalized x-component of the principal point.
     *
     * The x-component was normalized through a division by the width of the
     * camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns Normalized x-component of the principal point.
     */
    VL_SDK_API double VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetCxNorm(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the normalized y-component of the principal point.
     *
     * The y-component was normalized through a division by the height of the
     * camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns Normalized y-component of the principal point.
     */
    VL_SDK_API double VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetCyNorm(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns whether the intrinsic parameters are valid.
     *
     * A intrinsic camera calibration used for tracking should always be valid.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns \c true, if the intrinsic calibration is valid;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlIntrinsicDataWrapper_GetCalibrated(
        vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Returns the calibration error.
     *
     * The reprojection error in pixel. This is interesting for evaluating the
     * quality of a camera calibration.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \returns The reprojection error in pixel.
     */
    VL_SDK_API double VL_CALLINGCONVENTION
        vlIntrinsicDataWrapper_GetCalibrationError(
            vlIntrinsicDataWrapper_t* intrinsicDataWrapper);

    /*!
     * \ingroup intrinsic
     * \brief Retrieves the radial distortion parameters.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \param k Double array with 5 elements for storing the distortion
     *        parameters.
     * \param elementCount Number of elements in the given array.
     *        This should be 5.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlIntrinsicDataWrapper_GetRadialDistortion(
            vlIntrinsicDataWrapper_t* intrinsicDataWrapper, double k[],
            unsigned int elementCount);

    /*!
     * \ingroup intrinsic
     * \brief Sets the radial distortion parameters.
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \param k Double array with 5 elements, which contains the distortion
     *        parameters.
     * \param elementCount Number of elements in the given array.
     *        This should be 5.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlIntrinsicDataWrapper_SetRadialDistortion(
            vlIntrinsicDataWrapper_t* intrinsicDataWrapper, const double k[],
            unsigned int elementCount);

    /*!
     * \ingroup intrinsic
     * \brief Computed the projection matrix from the intrinsic camera
     *        parameters.
     *
     * The returned matrix is stored in the following order
     * (column-major order):
     * \f[
     *  \begin{bmatrix}
     *   0 & 4 &  8 & 12\\
     *   1 & 5 &  9 & 13\\
     *   2 & 6 & 10 & 14\\
     *   3 & 7 & 11 & 15\\
     *  \end{bmatrix}
     * \f]
     *
     * \param intrinsicDataWrapper Pointer to an IntrinsicDataWrapper object.
     * \param nearFact Value for the near clipping plane.
     * \param farFact Value for the far clipping plane.
     * \param screenWidth Width of the screen.
     * \param screenHeight Height of the screen.
     * \param renderRotation How the rendering is rotated relative to the orientation of the images
     *        received from the VisionLib. E.g., if the image will be rendered in
     *        landscape-left mode and the images are also in landscape-left mode,
     *        then ::VL_RENDER_ROTATION_CCW_0 should be used. If the image will be rendered in
     *        portrait mode, but the images are in landscape-left mode, then
     *        VL_RENDER_ROTATION_CCW_270 should be used.
     * \param mode The mode defines how to handle mismatching aspect ratios.
     *        Right now the mode value is ignored, but later we will support
     *        different modes like 'cover' (scale the projection surface up
     *        until it covers the whole screen) and 'contain' (scale the
     *        projection surface down until it is completely contained inside
     *        the screen).
     * \param matrix Float array with 16 elements for storing the projection
     *        matrix.
     * \param matrixElementCount Number of elements in the given array.
     *        This should be 16.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlIntrinsicDataWrapper_GetProjectionMatrix(
            vlIntrinsicDataWrapper_t* intrinsicDataWrapper,
            float nearFact, float farFact, unsigned int screenWidth,
            unsigned int screenHeight, unsigned int renderRotation,
            unsigned int mode, float matrix[], unsigned int matrixElementCount);

    /*!
     * \defgroup worker Worker
     *
     * \brief Functions for managing Worker objects.
     *
     * A Worker object controls the tracking thread.
     */

    /*!
     * \ingroup worker
     * \brief Creates a Worker object.
     *
     * Use ::vlDelete_Worker after usage to avoid memory leaks.
     *
     * \param aap Pointer to an AbstractApplicationWrapper object.
     * \returns Pointer to an Worker object. Use vlDelete_Worker after usage to
     *          avoid memory leaks.
     */
    VL_SDK_API vlWorker_t* VL_CALLINGCONVENTION vlNew_Worker(
        vlAbstractApplicationWrapper_t* aap);

    /*!
     * \ingroup worker
     * \brief Creates a synchronous Worker object.
     *
     * A synchronous Worker object doesn't create a new thread. Instead one
     * has to explicitly tell the Worker when to do his work by calling the
     * vlWorker_RunOnceSync function. This has the advantage, that we know
     * exactly when the tracking is running, when it's finished and when we
     * can get the results.
     *
     * If you want to call vlWorker_RunOnceSync from a different thread, then
     * you must call vlAbstractApplicationWrapper_RegisterThread first.
     *
     * Use ::vlDelete_Worker after usage to avoid memory leaks.
     *
     * \param aap Pointer to an AbstractApplicationWrapper object.
     * \returns Pointer to a synchronous Worker object. Use vlDelete_Worker
                after usage to avoid memory leaks.
     */
    VL_SDK_API vlWorker_t* VL_CALLINGCONVENTION vlNew_SyncWorker(
        vlAbstractApplicationWrapper_t* aap);

    /*!
     * \ingroup worker
     * \brief Deletes a Worker object.
     *
     * \param worker Pointer to a Worker object.
     */
    VL_SDK_API void VL_CALLINGCONVENTION vlDelete_Worker(vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Starts the tracking thread.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_Start(vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Stops the tracking thread.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_Stop(vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Processes the enqueued commands and the tracking once.
     *
     * This function only works, if the Worker was created as synchronous
     * instance (vlNew_SyncWorker). The target number of FPS will get ignored.
     * After calling this function you should call vlWorker_ProcessEvents and
     * vlWorker_PollEvents to invoke callbacks and registered listeners.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false, otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RunOnceSync(
        vlWorker_t* worker);

    /*!
    * \ingroup worker
    * \brief Processes the passed command.
    *
    * This function only works, if the Worker was created as synchronous
    * instance (vlNew_SyncWorker).
    *
    * \param worker Pointer to a Worker object.
    * \param jsonString Command as zero terminated JSON string.
    * \param callback Callback function, which will be called
    *        after the command was processed.
    * \param clientData The callback function will be called with the given
    *        pointer value as parameter. This can be used to invoke a member
    *        function.
    * \returns \c true, on success; \c false, otherwise.
    */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_ProcessJsonCommandSync(
        vlWorker_t* worker, const char jsonString[],
        vlCallbackJsonString callback, void* clientData);

    /*!
     * \ingroup worker
     * \brief Returns a pointer to the camera image.
     *
     * This function only works, if the Worker was created as synchronous
     * instance (vlNew_SyncWorker).
     *
     * The worker is the owner of the image. Therefore you should not call
     * ::vlDelete_ImageWrapper with the returned image as parameter.
     *
     * NOTICE: This functions is experimental and might get removed in future.
     *
     * \param worker Pointer to a Worker object.
     * \returns A pointer to an ImageWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlImageWrapper_t* VL_CALLINGCONVENTION vlWorker_GetImageSync(
        vlWorker_t* worker);

    /*!
    * \ingroup worker
    * \brief Returns a pointer to the camera image with the given name.
    *
    * This function only works, if the Worker was created as synchronous
    * instance (vlNew_SyncWorker).
    *
    * The worker is the owner of the image. Therefore you should not call
    * ::vlDelete_ImageWrapper with the returned image as parameter.
    *
    * NOTICE: This functions is experimental and might get removed in future.
    *
    * \param worker Pointer to a Worker object.
    * \param image_name name of the image to get
    * \returns A pointer to an ImageWrapper object, on success;
    *          \c NULL otherwise.
    */
    VL_SDK_API vlImageWrapper_t* VL_CALLINGCONVENTION
        vlWorker_GetImageByNameSync(
            vlWorker_t* worker, const char* image_name);

    
    /*!
     * \ingroup worker
     * \brief Returns a pointer to the camera image with the given name from a node.
     *
     * This function only works, if the Worker was created as synchronous
     * instance (vlNew_SyncWorker).
     *
     * The worker is the owner of the image. Therefore you should not call
     * ::vlDelete_ImageWrapper with the returned image as parameter.
     *
     *
     * \param worker Pointer to a Worker object.
     * \param node name of the node
     * \param key Name of the exposed image
     * \returns A pointer to an ImageWrapper object, on success;
     *          \c NULL otherwise.
     */
    VL_SDK_API vlImageWrapper_t* VL_CALLINGCONVENTION
    vlWorker_GetImageFromNodeSync(vlWorker_t* worker, const char node[], const char key[]);
    
    /*!
     * \ingroup worker
     * \brief Returns whether the thread is currently running or not.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, if the thread is running; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_IsRunning(
        vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Enqueues a command for the tracking thread as zero terminated
     *        string.
     *
     *  The command gets processed asynchronously by the tracking thread and a
     *  callback will called once after the processing has finished.
     *
     *  The following tracking independent commands exist:
     *  * createTracker &lt;trackingFile&gt;: Creates a tracker from a vl-file.
     *  * createTrackerFromString &lt;trackingString&gt;: Creates a tracker
     *    from a vl-string.
     *  * createLineTracker &lt;trackingFile&gt;: Creates a model tracker.
     *  * runTracking: Starts the tracking.
     *  * pauseTracking: Stops the tracking.
     *  * setTargetFPS &lt;fps&gt;: Sets the target number of frames per
     *    seconds of the tracking thread.
     *
     *  The following commands exist for the model tracker:
     *  * resetSoft: Resets the tracking.
     *  * resetHard: Resets the tracking and all keyframes.
     *  * setInitPose: Set the initial pose. The parameter must have the
     *    following structure: t_{x} t_{y} t_{z} q_{x} q_{y} q_{z} q_{w}.
     *  * writeInitData: Writes the captured initialization data as file to
     *    disk. The optional parameter can be used to change the filename and
     *    path (e.g. "local_storage_dir:InitData_"). A time stamp and the file
     *    extension will be added automatically. The default value is
     *    "local_storage_dir:InitData_".
     *
     * \param worker Pointer to a Worker object.
     * \param name Name of the command.
     * \param parameter Parameter for the command. Value will be ignored, if no
     *        parameters are expected by the command.
     * \param callback Callback function, which will be called inside
     *        vlWorker_ProcessCallbacks after the command was processed.
     * \param clientData The callback function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, if the command was enqueue successfully;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_PushCommand(
        vlWorker_t* worker, const char name[], const char parameter[],
        vlCallbackBool callback, void* clientData);

    /*!
     * \ingroup worker
     * \brief Enqueues a command for the tracking thread as zero terminated
     *        JSON string.
     *
     *  The command gets processed asynchronously by the tracking thread and a
     *  callback will called once after the processing has finished.
     *
     *  The following tracking independent commands exist:
     *  * Create a tracker from a vl-file
     *    \code{.json}
     *    {
     *        "name": "createTracker",
     *        "param": "<trackingFile>"
     *    }
     *    \endcode
     *    The callback issued by creating a tracker passes an error and a
     *    parameter string. If the error string is NULL, then no error
     *    occurred during the initialization. Otherwise the error will contain
     *    a zero terminated JSON string, such as:
     *    \code{.json}
     *    {
     *          "message":"some human readable message",
     *          "issues":[{"error":"someString",errorCode:1,level:2}]
     *    }
     *    \endcode
     *    If something happens, that will allow the tracking to take place, but
     *    does harm the users experience no error will be returned.
     *    In this case the parameter string will NOT be NULL and will contain a
     *    zero terminated JSON string, such as:
     *    \code{.json}
     *    {
     *          "issues":[{"error":"someString",errorCode:1}]
     *    }
     *    The error code will always describe the same type of error. A very
     *    common error is for example "2", which describes a not
     *    found / uncalibrated camera device, which could lead to unreliable
     *    tracking behaviour. The developer should handle this gracefully e.g.
     *    by offering to calibrate the camera. The device identifier that could
     *    not be found is set to the error string then.
     *
     *    The described behaviour is true for all commands that create a
     *    tracker.
     *
     *  * Create a new tracker from a vl-string
     *    \code{.json}
     *    {
     *        "name": "createTrackerFromString",
     *        "param": {
     *            "str": "<trackingConfigurationString>",
     *            "fakeFilename": "<fakeFilename>"
     *        }
     *    }
     *    \endcode
     *  * Create a new line tracker from a tracking configuration file
     *    \code{.json}
     *    {
     *        "name": "createLineTracker",
     *        "param": "<trackingFile>"
     *    }
     *    \endcode
     *  * Create a new line tracker from a tracking configuration string
     *    \code{.json}
     *    {
     *        "name": "createLineTrackerFromString",
     *        "param": {
     *            "str": "<trackingConfigurationString>",
     *            "fakeFilename": "<fakeFilename>"
     *        }
     *    }
     *    \endcode
     *  * Create a new poster tracker from a tracking configuration file
     *    \code{.json}
     *    {
     *        "name": "createPosterTracker",
     *        "param": "<trackingFile>"
     *    }
     *    \endcode
     *  * Create a new poster tracker from a tracking configuration string
     *    \code{.json}
     *    {
     *        "name": "createPosterTrackerFromString",
     *        "param": {
     *            "str": "<trackingConfigurationString>",
     *            "fakeFilename": "<fakeFilename>"
     *        }
     *    }
     *  * Start tracking
     *    \code{.json}
     *    {
     *        "name": "runTracking"
     *    }
     *    \endcode
     *  * Stop tracking
     *    \code{.json}
     *    {
     *        "name": "pauseTracking"
     *    }
     *    \endcode
     *  * Run the tracking once
     *    \code{.json}
     *    {
     *        "name": "runTrackingOnce"
     *    }
     *    \endcode
     *  * Set target number of frames per seconds of the tracking thread
     *    \code{.json}
     *    {
     *        "name": "setTargetFPS",
     *        "param": <fps>
     *    }
     *    \endcode
     *  * Get the current value of a certain attribute
     *    \code{.json}
     *    {
     *        "name": "getAttribute",
     *        "param": "<attribute name>"
     *    }
     *    \endcode
     *  * Set the value of a certain attribute
     *    \code{.json}
     *    {
     *        "name": "setAttribute",
     *        "param": {
     *            "att": "<attribute name>",
     *            "val": "<attribute value>"
     *        }
     *    }
     *    \endcode
     *
     *
     *  The following commands exist for the model tracker:
     *  * Reset the tracking
     *    \code{.json}
     *    {
     *        "name": "resetSoft"
     *    }
     *    \endcode
     *  * Reset the tracking and all keyframes
     *    \code{.json}
     *    {
     *        "name": "resetHard"
     *    }
     *    \endcode
     *  * Get the initial pose
     *    \code{.json}
     *    {
     *        "name": "getInitPose"
     *    }
     *    \endcode
     *  * Set the initial pose
     *    \code{.json}
     *    {
     *        "name": "setInitPose",
     *        "param": {
     *            "t": [x, y, z],
     *            "q": [x, y, z, w]
     *        }
     *    }
     *    \endcode
     *  * Write the captured initialization data as file to disk.
     *    \code{.json}
     *    {
     *        "name": "writeInitData",
     *        "param": "<filenamePrefix>"
     *    }
     *    \endcode
     *    "param" is optional. You can use it to change the filename and path
     *    (e.g. "param": "local_storage_dir:InitData_"). A time stamp and the
     *    file extension will be added automatically. The default value is
     *    "local_storage_dir:InitData_".
     *
     * \param worker Pointer to a Worker object.
     * \param jsonString Command as zero terminated JSON string.
     * \param callback Callback function, which will be called inside
     *        vlWorker_ProcessCallbacks after the command was processed.
     * \param clientData The callback function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, if the command was enqueue successfully;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_PushJsonCommand(
        vlWorker_t* worker, const char jsonString[],
        vlCallbackJsonString callback, void* clientData);

    /*!
     * \ingroup worker
     * \brief Enqueues a json command along with binary data for the visionLib.
     *
     *  The command gets processed asynchronously by the tracking thread and a
     *  callback will called once after the processing has finished.
     *
     *  Currently this command is used internally and will be documented in future versions.
     *  Thus it is considered as BETA!
     *
     * \param worker Pointer to a Worker object.
     * \param jsonString Command as zero terminated JSON string.
     * \param data Binary data pointer. The lifetime of the memory should be maintaned until the answer has been received. (might be 0)
     * \param size The size of the content passed.
     * \param callback Callback function, which will be called inside
     *        vlWorker_ProcessCallbacks after the command was processed.
     * \param clientData The callback function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, if the command was enqueue successfully;
     *          \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_PushJsonAndBinaryCommand(
        vlWorker_t* worker, const char jsonString[], const unsigned char data[],
        unsigned int size, vlCallbackJsonAndBinaryString callback,
        void* clientData);

   /*!
    * \ingroup worker
    * \brief Helper function for releasing a binary memory block.
    *
    * When an vlCallbackJsonAndBinaryString is issued, a memory block passed should be removed after using.
    *
    *  Currently this command is used internally and will be documented in future versions.
    *  Thus it is considered as BETA!
    *
    * \param data Pointer to a binary object.
    */
    VL_SDK_API void VL_CALLINGCONVENTION vlReleaseBinaryBuffer(const unsigned char data[]);

    /*!
     * \ingroup worker
     * \brief Executes all enqueued callbacks.
     *
     * Callbacks aren't called immediately from the tracking thread in order to
     * avoid synchronisation problems. Instead this method should be called
     * regularly from the main thread.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_ProcessCallbacks(
        vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Registers a listener for image events.
     *
     * \param worker Pointer to a Worker object.
     * \param p Listener function which will be notified during the
     *        event processing, if an image event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddImageListener(
        vlWorker_t* worker, vlCallbackImageWrapper listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from image events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveImageListener(
        vlWorker_t* worker, vlCallbackImageWrapper listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for extrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which will be notified during the
     *        event processing, if an extrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddExtrinsicDataListener(
        vlWorker_t* worker, vlCallbackExtrinsicDataWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from extrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveExtrinsicDataListener(
        vlWorker_t* worker, vlCallbackExtrinsicDataWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for intrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which will be notified during the
     *        event processing, if an intrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddIntrinsicDataListener(
        vlWorker_t* worker, vlCallbackIntrinsicDataWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from intrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveIntrinsicDataListener(
        vlWorker_t* worker, vlCallbackIntrinsicDataWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for tracking state events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which will be notified during the
     *        event processing, if a tracking state event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddTrackingStateListener(
        vlWorker_t* worker, vlCallbackZString listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from tracking state events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveTrackingStateListener(
        vlWorker_t* worker, vlCallbackZString listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for performance information events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which will be notified during the
     *        event processing, if a performance information event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddPerformanceInfoListener(
        vlWorker_t* worker, vlCallbackZString listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from performance info events.
     *
     * \param worker Pointer to a Worker object.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemovePerformanceInfoListener(
        vlWorker_t* worker, vlCallbackZString listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for named image events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested image as zero terminated string.
     * \param listener Listener function which will be notified during the
     *        event processing, if a named image event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddNamedImageListener(
        vlWorker_t* worker, const char key[], vlCallbackImageWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from named image events events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested image.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveNamedImageListener(
        vlWorker_t* worker, const char key[], vlCallbackImageWrapper listener,
        void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for named extrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested extrinsic data as zero terminated string.
     * \param listener Listener function which will be notified during the
     *        event processing, if a named extrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_AddNamedExtrinsicDataListener(
            vlWorker_t* worker, const char key[],
            vlCallbackExtrinsicDataWrapper listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from named extrinsic data events events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested extrinsic data.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_RemoveNamedExtrinsicDataListener(
            vlWorker_t* worker, const char key[],
            vlCallbackExtrinsicDataWrapper listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Registers a listener for named intrinsic data events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested intrinsic data as zero terminated string.
     * \param listener Listener function which will be notified during the
     *        event processing, if a named intrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_AddNamedIntrinsicDataListener(
            vlWorker_t* worker, const char key[],
            vlCallbackIntrinsicDataWrapper listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Unregisters a listener from named intrinsic data events events.
     *
     * \param worker Pointer to a Worker object.
     * \param key Key of the requested intrinsic data.
     * \param listener Listener function which should be unregistered.
     * \param clientData Pointer value used as parameter during the
     *        registration of the listener.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_RemoveNamedIntrinsicDataListener(
            vlWorker_t* worker, const char key[],
            vlCallbackIntrinsicDataWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Register a listener for named image events produced by a certain
     *        node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which will be notified during the
     *        event processing, if an image event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_AddNodeDataImageListener(
        vlWorker_t* worker, const char node[], const char key[],
        vlCallbackImageWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Unregister a listener from named image events produced by a
     *        certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which should be unregistered.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_RemoveNodeDataImageListener(
        vlWorker_t* worker, const char node[], const char key[],
        vlCallbackImageWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Register a listener for named extrinsic data events produced by a
     *        certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which will be notified during the
     *        event processing, if an extrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_AddNodeDataExtrinsicDataListener(
            vlWorker_t* worker, const char node[], const char key[],
            vlCallbackExtrinsicDataWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Unregister a listener from named extrinsic data events produced
     *        by a certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which should be unregistered.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_RemoveNodeDataExtrinsicDataListener(
            vlWorker_t* worker, const char node[], const char key[],
            vlCallbackExtrinsicDataWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Register a listener for named intrinsic data events produced by a
     *        certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which will be notified during the
     *        event processing, if an intrinsic data event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_AddNodeDataIntrinsicDataListener(
            vlWorker_t* worker, const char node[], const char key[],
            vlCallbackIntrinsicDataWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Unregister a listener from named intrinsic data events produced
     *        by a certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param key Name of the image to be addressed within the node.
     * @param listener Listener function which should be unregistered.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_RemoveNodeDataIntrinsicDataListener(
            vlWorker_t* worker, const char node[], const char key[],
            vlCallbackIntrinsicDataWrapper listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Register a listener for tracking state events produced by a
     *        certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param listener Listener function which will be notified during the
     *        event processing, if an tracking state event occurred.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_AddNodeTrackingStateListener(vlWorker_t* worker,
            const char node[], vlCallbackZString listener, void* clientData);

    /*!
     * \internal
     * \ingroup worker
     * \brief Unregister a listener from tracking state events produced by a
     *        certain node.
     *
     * @param worker Pointer to a Worker object.
     * @param node Name of the node to be addressed.
     * @param listener Listener function which should be unregistered.
     * \param clientData The listener function will be called with the given
     *        pointer value as parameter. This can be used to invoke a member
     *        function.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION
        vlWorker_RemoveNodeTrackingStateListener(vlWorker_t* worker,
            const char node[], vlCallbackZString listener, void* clientData);

    /*!
     * \ingroup worker
     * \brief Removes all listeners.
     *
     * @param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_ClearListeners(
        vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Calls the registered listeners for the enqueued events.
     *
     * Listeners aren't called immediately from the tracking thread in order to
     * avoid synchronisation problems. Instead this method should be called
     * regularly from the main thread.
     *
     * \param worker Pointer to a Worker object.
     * \returns \c true, on success; \c false otherwise.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_PollEvents(
        vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief Waits for enqueued events and calls the registered listeners.
     *
     * Listeners aren't called immediately from the tracking thread in order to
     * avoid synchronisation problems. Instead this method should be called
     * regularly from the main thread.
     *
     * \param worker Pointer to a Worker object.
     * \param timeout Number of milliseconds before stopping to wait. Under
              normal circumstances this shouldn't happen, but in case
              something went wrong, we don't want to wait indefinitely.
     * \returns \c true, on success or \c false, if there was an error while
     *          waiting for events. \c false is also returned, if the tracking
     *          is enabled, but timeout elapsed without an event arriving.
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_WaitEvents(
        vlWorker_t* worker, unsigned int timeout);

    /*!
     * \ingroup worker
     * \brief For testing purposed. Don't use!
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_Lock(vlWorker_t* worker);

    /*!
     * \ingroup worker
     * \brief For testing purposed. Don't use!
     */
    VL_SDK_API bool VL_CALLINGCONVENTION vlWorker_Unlock(vlWorker_t* worker);

    /*!
     * \defgroup Utils
     *
     * \brief Functions for general purpose and supporting your app.
     *
     * Functions allowing to support your implementation with the visionlibSDK.
     */

    /*!
     * \ingroup Utils
     * \brief Retrieves a file and its data from a given URI.
     *
     * This function tries to retrieve a file synchronously from a given source
     * URI. An AbstractApplication instance has to be created before calling
     * this function. The URI can point to a file on disk (file:///path) or
     * even to a file on a web server (http://192.0.0.1/file). VisionLib
     * schemes (e.g. project_dir:filename) and custom schemes are also
     * supported.
     *
     * \param uri A zero terminated string containing the URI.
     * \param size A pointer to an unsigned long integer, which will be used
     *        to store the size of the retrieved file.
     * \param options An optional pointer (can be NULL), which describes some
     *        options for download (not used yet).
     * \returns A valid pointer to the memory which MUST be freed using free(),
     *          on success. Returns \c NULL if an error occurred.
     */
     VL_SDK_API unsigned char* VL_CALLINGCONVENTION vlSDKUtil_get(
        const char *uri, unsigned long *size, const char *options=0);

    /*!
     * \ingroup Utils
     * \brief Posts or writes data to given URI.
     *
     * This function tries to write a file synchronously to a given URI. An
     * AbstractApplication instance has to be created before calling this
     * function. The URI can point to a file on disk (file:///path) or
     * even to a file on a web server (http://192.0.0.1/file). VisionLib
     * schemes (e.g. data_dir:filename) and custom schemes are also
     * supported.
     *
     * \param uri A zero terminated string containing the URI.
     * \param data A pointer to the data to be written.
     * \param size Number of bytes to be written.
     * \param options An optional pointer (can be NULL), which describes some
     *        options for download (not used yet)
     * \returns \c true, on success; \c false, otherwise.
     */
     VL_SDK_API bool VL_CALLINGCONVENTION vlSDKUtil_set(
        const char* uri, const void* data, unsigned long size,
        const char* options=0);

    /*!
     * \ingroup Utils
     * \brief Generate a temporary file URI.
     *
     * This function creates a filename that can be written to on the local
     * file system of the device. An AbstractApplication instance has to be
     * created before calling this function. A hint can be given, which will
     * get incorporated into the filename.
     *
     * \param prefName A zero terminated string containing the preferred name,
     *        which can also be NULL.
     * \param newName Buffer for storing the filename as zero terminated string.
     * \param maxSize Size of the buffer.
     * \returns \c true, on success; \c false, otherwise.
     */
     VL_SDK_API bool VL_CALLINGCONVENTION vlSDKUtil_getTempFilename(
        const char* prefName, char *newName, unsigned int maxSize);

    /*!
      * \ingroup Utils
      * \brief Transforms a workspace geometry into a list of points
      *
      * This function parses a workspace defined in json and transforms it into
      * an array of positions (3 float values).
      *
      * \param geometryJson A zero terminated string containing the geometry 
      *       definition as json.
     * \param size A pointer to an unsigned long integer, which will be used
     *        to store the size of the retrieved file.
      * \returns A valid pointer to the memory which MUST be freed using free(),
      *       on success. Returns \c NULL if an error occurred.
      */
     VL_SDK_API unsigned char* VL_CALLINGCONVENTION
     //VL_SDK_API float* VL_CALLINGCONVENTION
         vlSDKUtil_getCameraPositionsFromGeometry(
             const char* geometryJson, unsigned long* size);

     /*!
      * \ingroup Utils
      * \brief Transforms a workspace definition into a list of poses
      *
      * This function parses a workspace defined in json and transforms it into
      * an array of camera transformations (7 float values tx, ty, tz, rx, ry, rz, rw).
      *
      * \param geometryJson A zero terminated string containing the geometry
      *       definition as json.
      * \param size A pointer to an unsigned long integer, which will be used
      *        to store the size of the retrieved file.
      * \returns A valid pointer to the memory which MUST be freed using free(),
      *       on success. Returns \c NULL if an error occurred.
      */
     VL_SDK_API unsigned char* VL_CALLINGCONVENTION
         // VL_SDK_API float* VL_CALLINGCONVENTION
         vlSDKUtil_getCameraTransformsFromWorkspaceDefinition(
             const char* workspaceJson,
             unsigned long* size);

#ifdef __cplusplus
}
#endif // __cplusplus

/**@}*/

#endif // VL_SDK_H
