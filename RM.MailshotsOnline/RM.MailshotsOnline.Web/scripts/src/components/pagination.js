define(['jquery', 'knockout'],

    function($, ko) {

        // ViewModel
        function paginationComponentViewModel(params) {
            var self = this;

            this.data = params.data;
            this.displayedList = params.displayedList;
            this.pageNumber = ko.observable(0);
            this.perPage = params.perPage;
            this.flippedPagination = ko.observable(false);
            this.pagesArray = ko.observableArray([]);
            this.visibleNumbers = ko.observableArray([]);
            this.totalPages = ko.computed(function(){
                var pages = Math.floor(self.data().length / self.perPage),
                    pageNumbers;

                pages += self.data().length % self.perPage > 0 ? 1 : 0;

                pageNumbers = pages - 1;
                self.pagesArray.removeAll();
                for(var i=0; i < pageNumbers + 1; i++) {
                    self.pagesArray.push(i + 1);
                }

                return pages - 1;
            });

            this.pageController = function(targetPage) {
                return self.pageNumber(targetPage -1);
            };

            this.calcList = function() {
                var first = self.pageNumber() * self.perPage;
                this.displayedList(self.data().slice(first, first + self.perPage));
            };

            this.calcVisNumbers = function() {
                this.flippedPagination(false);
                var i;
                
                if (this.pagesArray().length < 5) {
                    return;

                } else {
                    this.visibleNumbers([]);

                    if (this.pageNumber() < 2) {
                        this.visibleNumbers([1,2,3]);

                    } else if (this.pageNumber() > this.pagesArray().length - 3) {
                        this.flippedPagination(true);
                        for(i=0; i < this.pagesArray().length; i++) {
                            if (i > this.pagesArray().length - 4) {
                                this.visibleNumbers.push(this.pagesArray()[i]);
                            }
                        }

                    } else {
                        for(i=0; i < this.pagesArray().length; i++) {
                            if (i > this.pageNumber() -2 && i < this.pageNumber() + 2) {
                                this.visibleNumbers.push(this.pagesArray()[i]);
                            }
                        }
                    }
                }

            };

            this.hasPrevious = ko.computed(function() {
                return self.pageNumber() !== 0;
            });

            this.hasNext = ko.computed(function() {
                return self.pageNumber() !== self.totalPages();
            });

            this.next = function() {
                if(self.pageNumber() < self.totalPages()) {
                    self.pageNumber(self.pageNumber() + 1);
                }
            };
            
            this.previous = function() {
                if(self.pageNumber() !== 0) {
                    self.pageNumber(self.pageNumber() - 1);
                }
            };

            this.data.subscribe(this.calcList, this);
            this.data.subscribe(this.calcVisNumbers, this);

            this.pageNumber.subscribe(this.calcList, this);
            this.pageNumber.subscribe(this.calcVisNumbers, this);

            this.calcList();
            this.calcVisNumbers();
        }

        return {
            viewModel: paginationComponentViewModel,
            template: { require: 'text!/scripts/src/templates/pagination.html' }
        };
});