# DataAccessLayer
A simple data access layer separation library.

#Getting Started
The Data Access Layer library is designed to be small modular. The core units of the library are Queries, Commands, and the DataAccessLayer itself. Queries are broken into two types the `DataAccess.Query<T>.WithoutTransform` and the `DataAccess.Query<T>.WithTransform<TOut>`. We will take a look at these two classes in more depth.

## DataAccess.Query.WithoutTransform
