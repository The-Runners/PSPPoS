# PSPPoS Web API

Point of sale system for software design course.

## Prerequisites

List of software you will need to install before starting development:

- Docker

## Domain logic

### Order

The order is the main entity of this system and goes through various stages during the process of ordering.

`Created` - this is the initial status of the order. During this phase, it should be possible to add products and services and change the order in other ways like adding a tip or changing the discount.

`Ordered` - this status indicates that the ordering is finished and no order details can be changed (this includes tips and discount). This status also indicates that payments can be made for the order as it is no longer going to change in price.

`Completed` - this status indicates that the order is fully paid for and thus is completed. Nothing about the order can change once it is in this status.

`Cancelled` - this status indicates that something went wrong and the order will not be completed. Likewise this means that no order details can be changed.
