# ActiveMarker - Active marker tracking and localization with image processing
Originally started out as my bachelor thesis, this system is a decoder service for active LED-based markers with error tolerance.

## Introduction

The main requirement for the applicability of mobile robots is the accurate tracking and positioning. In addition to mobile robots, in an industrian environment the tracking of raw materials, semi-finished and finished products is also extremely important. The concept of **"digital twin"** in Industry 4.0 covers not only the operations performed on products, but also their real position.
There are several methods for marker-based tracking (radio, optical, active, and passive), all of which have different characteristics, advantages, and disadvantages. The goal of this thesis is to examine different encodings and algorithms that implement the tracking and decoding **of time-encoded active LED markers**.
The repository contains a simulation environment that generates signals using arbitrary parameters as well as encoding. In addition to the ideal operation, the simulator is also able to simulate real errors, such as erroneous detections, missed observations, errors due to timing deviations. Besides of that, the repository contains general algorithms to track and decode these signals, to compare the simulated ideal and the tracked results.

## Tasks

 - To get acquainted with the theoretical background of active marker tracking
 - Design of a software simulation environment
 - Development of signaling protocols
 - Implementation of tracking algorithms
 - Examination of the model in an ideal (disturbance-free) and disturbance-laden environment
 - Examining the functionality of the model, looking for possible improvements