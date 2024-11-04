# Different levels of z-indices in the webapp

## About

Because at this point, I have introduces many different layers of z-indices. It is a good idea to have a list of those layers here.

## The list of layers by z-index value

0 - all the usual components

1000 - the blazor error message bottom row

1050 - Blazor Bootstrap modals (and probably offcanvases by default)

2000 - the overlay for marking bugs on the webpage

3000 - the top row with buttons to report bug, show tasks list etc.

4000 - tasks list

10000 - elements marked with always visible for testing