#ifndef VL_ERRORS_H
#define VL_ERRORS_H

/**
 * \file vlErrors.h
 * \brief This file contains error codes in an enum.
 *
 * You can use those error codes for comparison when receiving an error from a callback.
 */

/*!
 * Error codes.
 */
typedef enum
{
    /*! No information of the device name passed in the configuration could be loaded. The info string holds the device name requested. */
    VL_ERROR_DEVICE_NAME_LOAD_FAILED = 3,
    /*! There is maybe no camera connected to your system. */
    VL_ERROR_NO_CAMERA_CONNECTED = 4,
    /*! The camera may be removed or used by another application.*/
    VL_ERROR_NO_CAMERA_ACCESS = 5,
    /*! The file specified could not be acquired or loaded. */
    VL_ERROR_FILE_READING_FAILED = 98,
    /*! The file is not handable or has the wrong format. */
    VL_ERROR_FILE_INVALID = 99,
    /*! You have passed a file different to the ones allowed or it is not valid. So no valid configuration could be loaded. */
    VL_ERROR_FILE_FORMAT_NOT_ALLOWED = 100,
    /*! Will be issues if the license is not a valid file. */
    VL_ERROR_LICENSE_INVALID = 101,
    /*! Will be issues when the license has been expired. In this case a black image will be shown with the watermark instead of the camera image. The extrinsic data cannot be aquired. */
    VL_ERROR_LICENSE_EXPIRED = 102,
    /*! Will be issues when you have started the license more than 5 times without re-deploying. In this case a black image will be shown with the watermark instead of the camera image. The extrinsic data cannot be aquired. */
    VL_ERROR_LICENSE_EXCEEDS_RUNS = 103,
    /*! Will be issues if the license is not registered for this id. If you have licensed a commercial or poc license you might send us this id for updating your license file. */
    VL_ERROR_LICENSE_INVALID_HOST_ID = 105,
    /*! When the given license is not valid for running for the current platform (e.g. on HoloLens). */
    VL_ERROR_LICENSE_INVALID_PLATFORM = 107,
    /*! Will be issued if the specified license file has not been found at the specified location. */
    VL_ERROR_LICENSE_FILE_NOT_FOUND = 109,
    /*! Will be issues if the license is encountering a program version incompatibility (e.g. Lifetime License bound to a certain version). */
    VL_ERROR_LICENSE_INVALID_PROGRAM_VERSION = 110,
    /*! Will be issues if the license is encountering a required SEAT license but is not authorized (e.g PoC licenses are bound to Seats). */
    VL_ERROR_LICENSE_INVALID_SEAT = 111,
    /*! Will be issued if a certain feature of the visionLib is not available due to license restrictions. */
    VL_ERROR_LICENSE_INVALID_FEATURE = 112,
    /*! Will be issues if the license is not registered for this bundle id. If you have licensed a commercial or poc license you might send us this id for updating your license file. */
    VL_ERROR_LICENSE_INVALID_BUNDLE_ID = 114,
    /*! The model name passed could not be loaded. Info holds the filename uri requested. */
    VL_ERROR_MODEL_LOAD_FAILED = 300,
    /*! The model downloaded by the specified fileURI could not be decoded. */
    VL_ERROR_MODEL_DECODE_FAILED = 301,
    /*! The setup of the graph failed with an unknown reason. */
    VL_ERROR_GRAPH_SETUP_FAILED_UNKNOWN_ERROR = 400,
    /*! Could not find the node with the given name. Info holds the name of the node.*/
    VL_ERROR_GRAPH_NODE_NOT_FOUND = 401,
    /*! The data path doesn't comply with the expected pattern `nodeName.dataName`. Info holds the data path that was invalid.*/
    VL_ERROR_GRAPH_INVALID_DATA_PATH = 402,
    /*! Could not find an input of a node. Info contains: The nodename :: The Key that has not been found :: The keys that are defined on that node. */
    VL_ERROR_GRAPH_INPUT_NOT_FOUND = 403,
    /*! Could not find an output of a node. Info contains: The nodename :: The Key that has not been found :: The keys that are defined on that node. */
    VL_ERROR_GRAPH_OUTPUT_NOT_FOUND = 404,
    /*! There is a cycle in the graph - so no order of execution could be determined. */
    VL_ERROR_GRAPH_HAS_CYCLES = 405,
    /*! There was no tracker defined. */
    VL_ERROR_GRAPH_TRACKERS_EMPTY = 406,
    /*! The same name has been used for two or more devices. Info contains the name. */
    VL_ERROR_GRAPH_DUPLICATE_DEVICE_NAME = 407,
    /*! The same name has been used for two or more trackers. Info contains the name. */
    VL_ERROR_GRAPH_DUPLICATE_TRACKER_NAME = 408,

    /*! No calibration available for device. The device ID is passed in the info field. Please be aware that some standard calibration might be used which will can harm the tracking quality massively. */
    VL_WARNING_CALIBRATION_MISSING_FOR_DEVICE = 2,
    /*! The calibration DB could not be loaded. */
    VL_WARNING_CALIBRATION_DB_LOAD_FAILED = 10,
    /*! The calibration DB is not valid JSON. */
    VL_WARNING_CALIBRATION_DB_INVALID = 11,
    /*! While loading a calibration data base an internal error occured reading the database. Please review the error log of the visionLib for more information! */
    VL_WARNING_CALIBRATION_DB_LOAD_ERROR = 12,
    /*! While loading a calibration data base the following deviceID has been overwritten. */
    VL_WARNING_CALIBRATION_DEVICE_ID_OVERWRITTEN_ON_LOAD = 13,
    /*! While loading a calibration data base the following deviceID has been overwritten due to a defined alternative deviceID in 'Source'. */
    VL_WARNING_CALIBRATION_DEVICE_ID_OVERWRITTEN_BY_ALTERNATIVE_ID = 14,
    /*! An optional permission is not set. The program might not run as expected.*/
    VL_WARNING_PERMISSION_NOT_SET = 97,
    /*! When no valid license for a model bound feature could be found. This will not load this model and will result in undefined tracking behaviour. You should contact your license provider for updating the model hash provided in the info string. */
    VL_WARNING_LICENSE_MODEL_BOUND_FEATURE_INVALD = 104,
    /*! When the loaded model has NOT been registerd in the license. */
    VL_WARNING_LICENSE_USING_UNREGISTERED_MODELS = 108,
    /*! When the license is only valid for less than a week. You should contact your license provider for obtaining a new license.*/
    VL_WARNING_LICENSE_EXPIRING_SOON = 113,

    /*! We have encountered an about to deprecate parameter which should be replaced by an other or at least reviewed. */
    VL_DEPRECATION_WARNING = 20
} vlErrorCode;

#endif // VL_ERRORS_H
