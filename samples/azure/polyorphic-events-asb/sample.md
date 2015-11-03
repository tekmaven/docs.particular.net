---
title: Polymorphic events with Azure Service Bus Transport
summary: This sample shows how handle polymorphic events with Azure Service Bus Transport
tags:
related:
- nservicebus/azure/azure-servicebus-transport
- nservicebus/messaging/publish-subscribe
- nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed
- samples/azure/azure-service-bus
---

## Prerequisites 

An environment variable named `SamplesAzureServiceBusConnection` that contains the connection string for the Azure Service Bus namespace.


## Azure Service Bus Transport

This sample utilizes the [Azure Service Bus Transport](/nservicebus/azure/azure-servicebus-transport.md).


## Code walk-through

This sample has two endpoints. 

* `Publisher` publishes `BaseEvent` and `DerivedEvent` events.
* `Subscriber` subscribes and handles `BaseEvent` and `DerivedEvent` events.

`DerivedEvent` event is derived from `BaseEvent` event. The difference between the two events is an additional piece of information provided with the `DerivedEvent` in form of the `Data` property.

<!-- import BaseEvent -->

<!-- import DerivedEvent -->


## Publisher

The `Publisher` will publish an event of `BaseEvent` or `DerivedEvent` type based on the input it receives from the console.


## Subscriber

By default, all events handled in `Subscriber` will be auto subscribed. Default topology subscription behavior will create 2 subscriptions, one for each event.

![](images/subscriptions.png)


### Auto subscription behavior

Normally, this would be fine. Though not with ASB transport and polymorphic events. Each subscription is filtering messages based on `NServiceBus.EnclosedMessageTypes` header. When an event of `BaseType` is published, it's going only into `Samples.ASB.Polymorphic.Subscriber.BaseEvent` subscription as per image below.

![](images/baseevent.published.png)

But whenever `DerivedEvent` event is published, both `Samples.ASB.Polymorphic.Subscriber.BaseEvent` and `Samples.ASB.Polymorphic.Subscriber.DerivedEvent` subscriptions get a copy of that message. 

![](images/derivedevent.published.png)

Since `DerivedEvent` implements `BaseEvent`, it's `NServiceBus.EnclosedMessageTypes` header will contain both types:

`Events.DerivedEvent, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;Events.BaseEvent, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null`

Both filters will pick the `DerivedEvent` message, causing duplicate delivery to the `Subscriber`. NServiceBus `Subscriber` endpoint will invoke handlers for each type that message implements. End result will be multiple invocations for the same message.

<!-- import PublisherOutput -->

<!-- import SubscriberOutput -->


### How to address this?

To address this in general and allow proper handling of polymorphic events, `Subscriber` has do the following:

1. Disable automatic subscription.
1. Subscribe explicitly to the base events only of polymorphic events.
1. Subscribe explicitly to the non-polymorphic events it's interested in.

<!-- import DisableAutoSubscripton -->

When an event is a polymorphic event, such as `DerivedEvent`, endpoint will subscribe to the **base event** only.

<!-- import ControledSubscriptions -->

For this sample, configuring `Subscriber` as described above, will create the topology that only has `BaseEvent` subscription serving as "catch-all".

![](images/single.subscription.png)

Results of the sample now adhere to the expected polymorphic message handling 

<!-- import PublisherOutput-from-sample -->

<!-- import SubscriberOutput-from-sample -->