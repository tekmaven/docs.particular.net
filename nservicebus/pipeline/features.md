---
title: Features
summary: Implement a Feature for advanced extension of NServiceBus
tags:
 - feature
 - pipeline
---

While NServiceBus provides some simple interfaces to plug in your code at certain steps in the life-cycle, Features offer a more complete approach to write and distribute custom extensions.

Features allow you to:

* Configure required dependencies for your Feature
* Enable or disable a Feature via configuration or based on conditions and dependencies
* Get full access to the pipeline, settings and the container


## Feature API

To create a new feature create a class which inherits from `Feature`. This offers you two basic extensibility points:

* The constructor of the class will always be executed and should be used to determine whether to enable or disable the feature, configure default settings and registering startup tasks.
* The `Setup` method is called if all defined conditions are met and the feature is marked as *enabled*. Use this method to configure and initialize all required components for your feature.

snippet:MinimalFeature


## Dependencies

Features can depend on each other to form more complex functionality.

snippet:DependentFeature

A feature might use either strongly or loosely typed API when declaring dependencies (the latter can be useful if a dependency is declared in an external assembly).

NOTE: The feature name is derived from the name of the type, however the suffix `Feature`, if present in the type name, is removed from the name of the feature.

The API also allows to declare optional dependencies on one or more of listed features.


## Enabling, disabling and activation

In order for a feature to take part in the bus construction it has to first become *enabled* and then, only if it has been enabled, qualify for *activation*. By default features are disabled unless explicitly enabled.

This can be overridden and a feature can be enabled by default (like most of the core features are):

snippet:FeatureEnabledByDefault


### Defaults

When a feature is found to be enabled, the bootstrapping code applies the *defaults* defined by that feature to a shared dictionary containing the settings.

snippet:FeatureWithDefaults

The code above configures the key `"Key"` to contain value `"Value"` and the key `"OtherKey"` to contain value `42` as a default value. The querying API allows to distinguish these two types of registrations. Usually, for a given key, a feature registers a default value in this way and also exposes an extension method of `BusConfiguration` to allow a user to override this default value.


### Enabling other features

The list of all the enabled features is built interactively. A feature can enable other feature via the *defaults* mechanism and that fact might trigger another set of *defaults* to be applied, enabling subsequent features etc.

snippet:EnablingOtherFeatures


### Enabling features from the outside

You can manually activate or deactivate your features and most of the internal NServiceBus features in your endpoint configuration with the `BusConfiguration`:

snippet:EnableDisableFeatures


### Prerequisites

When the enabling algorithm can't find any more feature that should be enabled, it kicks off the second stage which does the *prerequisite* checking. Each feature can declare the prerequisites as predicates that needs to be satisfied for that feature to be able to be activated

snippet:FeatureWithPrerequisites

NOTE: The differentiation between *explicit* settings and *default* settings comes useful when determining if a given feature should be activated.


### Activation

Final stage is the activation where each feature has its chance to set up the bus. The features are activated in order of dependencies which means that when a given feature is activating, all the features it depends on have already been activated.

In order for a feature to be activated it needs to satisfy the following criteria:
 * It needs to be *enabled*
 * All its *prerequisites* need to be satisfied
 * All the feature's dependencies need to be *enabled*
 * All the *prerequisites* of all the feature's dependencies need to be satisfied


## Feature setup

`Setup` has to be implemented in your feature and will be called if the feature is enabled. It allows you to complete configuration of your feature, hook into the pipeline or registering services on the container. The `FeatureConfigurationContext` parameter on the method contains:

* Settings: read or write settings which should be available to other components or read access settings provided by NServiceBus.
* Container: register services with the dependency injection container which can be injected to other components.
* Pipeline: register your own behavior into the message processing pipeline or replace/remove existing ones.

snippet:FeatureSetup


Note: Features are automatically detected and registered by NServiceBus when the assembly is scanned.


## Feature startup tasks

If you need to execute some feature related logic after the feature has been started or stopped you can do that by providing a `FeatureStartupTask` which comes with an `OnStart` and `OnStop` method. Also, the task will always be disposed after stopping the feature if it implements `IDisposable`.

snippet:FeatureStartupTaskDefinition

To associate a `FeatureStartupTask` with your feature, using `RegisterStartupTask`.

snippet:FeatureStartupTaskRegistration

The task will only be created and called if the feature is enabled. The `FeatureStartupTask`s are activated and started in random order.

Note: In Version 5 `FeatureStartupTask` are executed synchronously. Avoid long running operations which will delay the bus startup time.

Note: In Version 6 `FeatureStartupTask` run asynchronously the calling thread which initialized the endpoint. Furthermore, you get access to the `IBusContext` which allows to do basic bus operations inside a `FeatureStartupTask`.
