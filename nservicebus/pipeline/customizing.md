---
title: Customizing the Pipeline
summary: Customizing the message handling pipeline
tags:
- Pipeline
redirects:
- nservicebus/pipeline/customizing
---

NServiceBus has always had the concept of a pipeline execution order that is executed when a message is received or dispatched. From NServiceBus version 5 on, this pipeline has been made a first level concept and exposes it for extensibility. This allows users to take full control of the incoming and outgoing message processing.

Each pipeline is composed of "steps". A step is an identifiable value in the pipeline used to programmatically define order of execution. Each step represents a behavior which will be executed at the given place within the pipeline. You can add additional behavior to the pipeline by registering a new step or replace the behavior of an existing step with your custom implementation.

Note: Customizing the pipeline is an extremely powerful extensibility mechanism but also it can cause severe damage. Do not hesitate to contact us for help!

---



TODO: stages: groups steps
TODO: behavior context
TODO: upgrade guide


## How to code behaviors?

## How does the pipeline execute its steps?

The pipeline is implemented using the [Russian Doll](https://en.wikipedia.org/wiki/Matryoshka_doll) Model. Russian dolls are a series of progressively smaller dolls nested within each other. Similarly, the pipeline model is a series of progressively nested steps within each other.

### How to create a new step?
snippet:NewStepInPipeline
NOTE: TODO: split in define step for behavior and register step

### How to replace behavior of a built-in step?
snippet:ReplacePipelineStep


## Exception Handling


### MessageDeserializationException (Version 5.1 and up)

As of Version 5.1 if a message fails to deserialize a `MessageDeserializationException` will be thrown.


#### When to throw

The implementation of `DeserializeLogicalMessagesBehavior` handles deserialization and can throw `MessageDeserializationException`. So any behavior that replaces  `DeserializeLogicalMessagesBehavior` should duplicate this functionality.


#### When to handle

The implementation of `UnitOfWorkBehavior` handles aggregating multiple exceptions that can occur in a unit of work. However `MessageDeserializationException` is re-thrown (not aggregated). So any behavior that replaces  `UnitOfWorkBehavior` should duplicate this functionality.   
