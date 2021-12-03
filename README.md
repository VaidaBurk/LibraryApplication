# LibraryApplication
 A console application to manage book library.
 
 Comands to manage books: \n
 List books -> to print list of all books in the library
 Filter books list -> to print filtered list according to given parameter
 Add book -> to add new book to the library
 Delete book -> to delete book from library (please note, that borrowed book can be deleted too in case it was lost)
 
 Comands to manage borrowings:
 Take book -> to borrow bokk from the library
 Return book -> to return borrowed book

To borrow book from the library it is necessary to have a reader ID.
In this stage it is assumed that any reader ID which consists from numbers is valid. Another app should be responsible for creating readers and storing their data.
