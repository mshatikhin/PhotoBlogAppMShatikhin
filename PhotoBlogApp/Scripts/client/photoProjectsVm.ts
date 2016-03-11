module PhotoBlogApp
{

    export class PhotoProjectVm
    {
        showDescription = ko.observable(false);
        constructor(public photoProject: Api.PhotoProjectM, public toogleDescription)
        {
        }
    }

    export class PhotoProjectsVm
    {
        load: () => void;
        photoProjects = ko.observableArray<PhotoProjectVm>([]);
        toogleDescription: (projectVm: PhotoProjectVm) => void;

        constructor(target: HTMLElement)
        {
            this.toogleDescription = (projectVm) => {
                var show = !projectVm.showDescription();
                this.photoProjects().forEach((p) =>
                {
                    p.showDescription(false);
                });
                projectVm.showDescription(show);
            }

            this.load = () =>
            {
                Api.AsyncClient.Current.GetPhotoProjects((photoProjects) =>
                {
                    this.photoProjects(photoProjects.map((p) => { return new PhotoProjectVm(p, this.toogleDescription) }));
                });
            }

            ko.applyBindings(this, target);
        }
    }

}