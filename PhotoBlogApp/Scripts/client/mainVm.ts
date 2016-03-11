module PhotoBlogApp
{
    export class ValueVm
    {
        isValid = ko.observable(false);
        value = ko.observable("");

        constructor(public v: string, public onSetValidForm)
        {
            this.value(v);
            this.value.subscribe((newValue) =>
            {
                if (newValue.length > 0) {
                    this.isValid(true);
                } else {
                    this.isValid(false);
                }
                this.onSetValidForm();
            });
        }
    }

    export class AlbumVm
    {
        constructor(public album: Api.AlbumM) { }
    }

    export class PostVm
    {
        isCollapsed = ko.observable(false);
        expand: () => void;
        collapse: () => void;
        scroll = ko.observable(false);
        url = "";
        shareUrl = "";
        share: () => void;

        constructor(public post: Api.PostM)
        {
        }
    }

    export class MainPagePhotoVm
    {
        constructor(public url, public fullUrl)
        {
        }
    }

    export class MainPhotosVm
    {
        photoLeft: KnockoutObservable<MainPagePhotoVm> = ko.observable(null);
        photoRight: KnockoutObservable<MainPagePhotoVm> = ko.observable(null);
        private images = [];
        shuffled = true;
        shuffle: () => void;
        constructor()
        {
            var random = (min, max) =>
            {
                var num = Math.random() * (max - min) + min;
                return num.toFixed();
            }

            this.images = [
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_2247_.jpg", "//mshatikhin.ru/Content/img/main/img_2247.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_8367_.jpg", "//mshatikhin.ru/Content/img/main/img_8367.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_3544_.jpg", "//mshatikhin.ru/Content/img/main/img_3544.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_9431_.jpg", "//mshatikhin.ru/Content/img/main/img_9431.jpg")
            ];

            this.shuffle = () =>
            {
                this.photoLeft(this.images[random(0, 1)]);
                this.photoRight(this.images[random(2, 3)]);
            };

            this.shuffle();
        }
    }

    export class MainVm
    {
        init: () => void;

        imageUrl = ko.observable("");
        imageHeight = ko.observable(250);

        isValidForm = ko.observable(false);
        setValidForm: () => void;

        valueName: KnockoutObservable<ValueVm>;
        valueCoordinates: KnockoutObservable<ValueVm>;
        afterRender: () => void;
        mainPhotosVm = new MainPhotosVm();

        constructor(targetElem: HTMLElement, public hash: string)
        {
            this.setValidForm = () =>
            {
                this.isValidForm(this.valueName().isValid() && this.valueCoordinates().isValid());
            };

            this.valueName = ko.observable(new ValueVm("", this.setValidForm));
            this.valueCoordinates = ko.observable(new ValueVm("", this.setValidForm));

            this.init = () =>
            {
                Util.introAnimate();
            };

            this.afterRender = () =>
            {
                setTimeout(this.init(), 1);
            };

            ko.applyBindings(this, targetElem);

            $(document).on("click", ".img_js", (event) =>
            {
                this.imageUrl("");
                var url = $(event.target).attr("data-fullimage");
                if (url === null || url === undefined || url === "") {
                    url = $(event.target).attr("src");
                }
                var height = $(window).height();
                var imageHeight = height - (height * 0.1);
                this.imageUrl(url);
                this.imageHeight(imageHeight);
            });
        }
    }
} 