Example Project with Boring Business Case
====

This repository contains example repository that demonstrates TDD, BDD and some other guidelines that I like to follow.
The boring side is to create a simple application to give users movie recommendations based on their genre preference.


Use cases
===

**Scenario: Like a movie**\
**When** user likes a movie using API endpoint `/movie/like/{movie id}`\
**Then** add the liked movie to the user preference

**Scenario: Dislike a movie**\
**Given** the user has liked liked the movie\
**When** user dislikes a movie using API endpoint `/movie/dislike/{movie id}`\
**Then** remove the liked movie from the user preference

**Scenario: Get movie recommendations for user based on genre preference**\
**Given** user has at least one liked movie\
**When** user requests movie recommendations using API endpoint `//movie/recommend`\
**Then** extract unique genres from user's liked movies\
 **And** search movies from a movie library\
 **And** list a movie if the movie's genres contain any of the extracted genres
