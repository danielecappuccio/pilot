// ========================================================================= //
//                                                                           //
// Filename:  vlSDKVersion.h                                                 //
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
// Creation Date :  Tue Apr 12 11:06:00 2016   Martin Buchner                //
//                                                                           //
// ========================================================================= //

/**
 * \file vlSDKVersion.h
 * \brief This file contains defines with the VisionLib version.
 *
 * You can use those defines and the ::vlGetVersionMajor,
 * ::vlGetVersionMinor, ::vlGetVersionRevision, ::vlGetVersionString, 
 * ::vlGetVersionHashString and/or ::vlGetVersionTimestampString functions to 
 * check whether the binaries have the same version as the source files.
 *
 * \see http://www.visionlib.com
 */

#ifndef VL_SDK_VERSION_H
#define VL_SDK_VERSION_H

/*! Major version. */
#define VL_SDK_VERSION_MAJOR 19

/*! Minor version. */
#define VL_SDK_VERSION_MINOR 11

/*! Revision version. */
#define VL_SDK_VERSION_REVISION 1

/*! Version as string. */
#define VL_SDK_VERSION_STRING "19.11.1"

/*! Version hash as string. */
#define VL_SDK_VERSION_HASH_STRING "6c3a788d2213ac376af8ef6f9e9a69262862e29e"

/*! Creation timestamp of this version. */
#define VL_SDK_VERSION_TIMESTAMP_STRING "2019-11-07 08:16:47"

#endif
