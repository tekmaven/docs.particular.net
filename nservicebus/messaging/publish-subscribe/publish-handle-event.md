---
title: Publish and Handle an Event
summary: How to define a message as an event and publish that message.
tags:
- Publish Subscribe
related:
- samples/pubsub
- samples/step-by-step
- nservicebus/messaging/messages-events-commands
---


## Classifying a message as an event

To publish a message it must be classified as an event. There are two ways of achieving this 


### Via a Marker interface

Adding a marker interface to the message definition.

<!-- import EventWithInterface -->


### Via Message Conventions

Using the `EventWithConvention` message convention.

Given a message with the following definition.

<!-- import EventWithConvention -->

It could be treated as an event using the following convention. 

<!-- import DefiningEventsAs -->


## Handling a event

An event can be handled by use of the `IHandleMessages` interface on any [Handler](/nservicebus/handlers) or [Saga](/nservicebus/sagas).


## Publishing an event

An event can be published via any instance of `IBus`. However there are sever common locations where publishing occurs.


### From a Handler

From a handler in reaction to some other message being handled.

<!-- import publishFromHandler -->


### From a Saga

From a handler in reaction to some other message being handled.

<!-- import publishFromSaga -->


### At endpoint startup

At startup of an endpoint, directly after the bus has started.

<!-- import publishAtStartup -->


## Events as Classes or Interfaces

Events can be either classes or interfaces. Since interfaces cannot be constructed there are slightly different semantics for publishing each.


### Publish a class 

<!-- import InstancePublish -->


### Publish an interface

If you are using interfaces to define your event contracts you need to set the message properties by passing in a lambda. NServiceBus will then generate a proxy and set those properties. 

<!-- import InterfacePublish -->