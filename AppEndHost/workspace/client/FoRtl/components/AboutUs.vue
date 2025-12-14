<template>
    <section id="about-section" class="section bg-white">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-24">
                    <h2 class="section-title text-start mb-4">{{ translate("About Us") }}</h2>
                    <p class="lead">
                        {{ translate("AppEnd is a full-stack low-code platform designed to help developers and businesses build applications rapidly.") }}
                    </p>
                    <p class="mb-4">
                        {{ translate("With AppEnd, you can create APIs, user interfaces, and manage access levels without writing extensive code. The platform supports dynamic code compilation, template-based UI generation, and provides a complete development environment.") }}
                    </p>
                    <ul class="list-unstyled">
                        <li class="mb-3">
                            <i class="fa-solid fa-fw fa-check-circle text-success me-2"></i>
                            {{ translate("Open source and free") }}
                        </li>
                        <li class="mb-3">
                            <i class="fa-solid fa-fw fa-check-circle text-success me-2"></i>
                            {{ translate("Easy to learn and use") }}
                        </li>
                        <li class="mb-3">
                            <i class="fa-solid fa-fw fa-check-circle text-success me-2"></i>
                            {{ translate("Runs on Linux and Windows") }}
                        </li>
                        <li class="mb-3">
                            <i class="fa-solid fa-fw fa-check-circle text-success me-2"></i>
                            {{ translate("Full-stack application support") }}
                        </li>
                    </ul>
                </div>
                <div class="col-24 text-center">
                    <div class="about-image-placeholder p-5 bg-light rounded">
                        <i class="fa-solid fa-users fa-8x text-primary opacity-50"></i>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
let _this = { cid: "", c: null };

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this; 
    },
    mounted() { },
    methods: {
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
.about-image-placeholder {
    min-height: 300px;
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>

